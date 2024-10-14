using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventDeleted : IEvent
    {
        public Guid EventId { get; }
        public string EventName { get; }
        public Guid OrganizerId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public EventDeleted(Guid eventId, string eventName, Guid organizerId, DateTime startDate, DateTime endDate)
        {
            EventId = eventId;
            EventName = eventName;
            OrganizerId = organizerId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
