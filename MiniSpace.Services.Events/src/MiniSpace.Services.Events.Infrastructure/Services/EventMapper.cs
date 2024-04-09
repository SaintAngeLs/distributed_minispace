using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            // switch (@event)
            // {
            //     
            // }

            return null;
        }
    }
}