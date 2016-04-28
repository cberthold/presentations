using Infrastructure.Command;
using Infrastructure.Domain;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Commands;

namespace Todo.BoundedContext.Handlers
{
    public class RebuildReadModelCommandHandlers :
        ICommandHandler<RebuildReadModelCommand>
    {
        readonly Guid TenantId = Guid.Parse("2DF4109C-0B42-40C0-97B6-E9024EF47253");

        readonly IEventStore eventStore;
        readonly IEventPublisher publisher;

        public RebuildReadModelCommandHandlers(IEventStore eventStore, IEventPublisher publisher)
        {
            this.eventStore = eventStore;
            this.publisher = publisher;
        }

        public void Handle(RebuildReadModelCommand message)
        {
            var events = from e in eventStore.Get(TenantId, -1)
                         orderby e.Version
                         select e;
            foreach(var @event in events)
            {
                publisher.Publish(@event);
            }
        }
    }
}
