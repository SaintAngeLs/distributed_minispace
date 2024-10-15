using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventArchived : IEvent
    {
        public Guid EventId { get; }

        public EventArchived(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
