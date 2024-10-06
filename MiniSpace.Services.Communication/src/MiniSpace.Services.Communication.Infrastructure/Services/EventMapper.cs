using Paralax.CQRS.Events;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map).Where(mappedEvent => mappedEvent != null);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case MessageAddedEvent e:
                    return new MessageSent(e.ChatId, e.MessageId, Guid.Empty, string.Empty);
                
                // Add more cases for other domain events
                // case SomeOtherDomainEvent e:
                //     return new SomeOtherIntegrationEvent(...);
                
                default:
                    return null;
            }
        }
    }
}
