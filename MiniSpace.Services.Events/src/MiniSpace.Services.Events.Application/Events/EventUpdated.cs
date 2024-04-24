using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventUpdated(Guid eventId, DateTime updatedAt, Guid updatedBy) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public DateTime UpdatedAt { get; set; } = updatedAt;
        public Guid UpdatedBy { get; set; } = updatedBy;
    }
}