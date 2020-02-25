using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace Infrastructure
{
    public class SqlStreamStoreEventStoreAdapter : IEventStore
    {
        private readonly IStreamStore store;

        public SqlStreamStoreEventStoreAdapter(IStreamStore store)
        {
            this.store = store;    
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default)
        {
            var streamId = aggregateId.ToStreamId(); 
            const int MAX_COUNT = int.MaxValue;
            const bool PREFETCH_JSON_DATA = true;

            if(fromVersion < 0)
            {
                fromVersion = 0;
            }

            var page = await store.ReadStreamForwards(streamId, fromVersion, MAX_COUNT, PREFETCH_JSON_DATA, cancellationToken);
            
            var returnEvents = new List<IEvent>(page.Messages.Length * (page.IsEnd ? 1 : 2));

            while(true)
            {
                foreach(var message in page.Messages)
                {
                    var @event = await message.RehydrateEvent(cancellationToken);
                    returnEvents.Add(@event);
                }

                if(page.IsEnd)
                {
                    break;
                }

                page = await store.ReadStreamForwards(streamId, fromVersion, MAX_COUNT, PREFETCH_JSON_DATA, cancellationToken);
            }

            return returnEvents;
        }

        

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {

            var streams = from e in events.Select(CreateNewStreamMessageWrapper)
                           group e by e.AggregateId into grp
                           let grpIndex = new {
                               AggregateId = grp.Key,
                               FirstIndex = grp.OrderBy(a=> a.Index).First(),
                               Events = grp.OrderBy(a=> a.Version).ToArray(),
                           }
                           orderby grpIndex.FirstIndex.Index
                           let streamVersion = grpIndex.Events.First().Version - 1
                           let expectedVersion = (streamVersion == 0) ? ExpectedVersion.NoStream : streamVersion - 1
                           select new {
                               StreamId = grpIndex.AggregateId.ToStreamId(),
                               ExpectedVersion = expectedVersion,
                               Messages = grpIndex.Events.Select(a=>a.Message).ToArray(),
                           };
                           
            foreach(var stream in streams)
            {
                await store.AppendToStream(stream.StreamId, stream.ExpectedVersion, stream.Messages, cancellationToken);
            }
        }

        private static NewStreamMessageWrapper CreateNewStreamMessageWrapper(IEvent @event, int index)
        {
            var metadata = new {
                @event.TimeStamp,
            };
            Guid messageId = Guid.NewGuid();
            string eventType = @event.GetType().FullName;
            string jsonData = @event.SerializeData();
            string jsonMetadata = @metadata.SerializeData();
            return new NewStreamMessageWrapper
            {
                Index = index,
                AggregateId = @event.Id,
                Version = @event.Version,
                Message = new NewStreamMessage(messageId, eventType, jsonData, jsonMetadata),
            };
        }


        public class NewStreamMessageWrapper
        {
            public int Index { get; set; }
            public Guid AggregateId { get; set; }
            public int Version { get; set; }
            public NewStreamMessage Message { get; set; }
        }
    }

    public static class GuidExtensions
    {
        public static StreamId ToStreamId(this Guid input)
        {
            return new StreamId(input.ToString("d"));
        }
    }
}