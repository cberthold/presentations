using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;

namespace frontend.Logic.DomainEvents
{
    public class AbstractDomainEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public AbstractDomainEvent()
        {
        }
    }
}