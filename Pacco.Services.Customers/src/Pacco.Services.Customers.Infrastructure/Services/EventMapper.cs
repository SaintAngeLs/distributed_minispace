using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Customers.Application.Services;
using Pacco.Services.Customers.Core;
using Pacco.Services.Customers.Core.Events;

namespace Pacco.Services.Customers.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case CustomerRegistrationCompleted e: return new Application.Events.CustomerCreated(e.Customer.Id);
                case CustomerBecameVip e: return new Application.Events.CustomerBecameVip(e.Customer.Id);
                case CustomerStateChanged e:
                    return new Application.Events.CustomerStateChanged(e.Customer.Id,
                        e.Customer.State.ToString().ToLowerInvariant(), e.PreviousState.ToString().ToLowerInvariant());
            }

            return null;
        }
    }
}