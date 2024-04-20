using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventDeleted(Guid eventId) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
    }
}