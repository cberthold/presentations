using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace Infrastructure
{
    public class SqlStreamStoreEventStoreAdapter : IEventStore
    {
        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}