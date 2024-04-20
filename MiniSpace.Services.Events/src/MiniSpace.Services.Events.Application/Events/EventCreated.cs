using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventCreated(Guid eventId, Guid organizerId) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public Guid OrganizerId { get; set; } = organizerId;
    }
}