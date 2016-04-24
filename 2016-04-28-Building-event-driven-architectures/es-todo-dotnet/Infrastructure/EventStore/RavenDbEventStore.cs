using Infrastructure.Events;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EventStore
{
    public class RavenDbEventStore : IEventStore
    {
        private static IDocumentStore store;
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        static RavenDbEventStore()
        {

            store = new EmbeddableDocumentStore
            {
                ConnectionStringName = "EventStoreDb"
            };
            store.Initialize();
        }

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            // open the document store session
            using (var session = store.OpenSession())
            {
                // create a query based on DocumentData entity
                var query = session.Query<DocumentData>()
                    // wait for stale indexes to complete
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow());

                // get all documents
                var documents = from d in query
                                    // where our aggregate id matches
                                where d.AggregateId == aggregateId
                                // and version is greater than our
                                // beginning version 
                                && d.Version > fromVersion
                                // order by the version
                                orderby d.Version
                                select d;

                // create a function to deserialize event data
                Func<string, IEvent> func = (data) =>
                {
                    var output = JsonConvert.DeserializeObject(data, settings);

                    return (IEvent)output;
                };

                // deserialize the events from our documents
                var events = from e in documents.ToList()
                             select func(e.EventData);

                // return the data as events
                return events.Cast<IEvent>();
            }
        }

        public void Save(IEvent @event)
        {
            var document = new DocumentData()
            {
                CommitId = Guid.NewGuid(),
                AggregateId = @event.Id,
                Version = @event.Version,
                EventData = JsonConvert.SerializeObject(@event, settings)
            };

            using (var session = store.OpenSession())
            {
                session.Store(document);
                session.SaveChanges();
            }
        }

        public class DocumentData
        {
            public Guid CommitId { get; set; }
            public Guid AggregateId { get; set; }
            public int Version { get; set; }
            public string EventData { get; set; }
        }

    }
}
