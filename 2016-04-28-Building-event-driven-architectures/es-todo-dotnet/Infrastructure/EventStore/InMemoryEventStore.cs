﻿using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDB = new Dictionary<Guid, List<IEvent>>();

        public void Begin()
        {
            
        }

        public void Commit()
        {
            
        }

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            List<IEvent> events;
            _inMemoryDB.TryGetValue(aggregateId, out events);
            return events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>();
        }

        public void Save(IEvent @event)
        {
            List<IEvent> list;
            _inMemoryDB.TryGetValue(@event.Id, out list);
            if (list == null)
            {
                list = new List<IEvent>();
                _inMemoryDB.Add(@event.Id, list);
            }
            list.Add(@event);
        }
    }
}
