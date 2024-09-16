using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events
{
    [Message("events")]
    public class EventDeleted : IEvent
    {
        public Guid EventId { get; }
        public string EventName { get; }
        public Guid OrganizerId { get; }
        public string OrganizerType { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public EventDeleted(Guid eventId, string eventName, Guid organizerId, 
            string organizerType, DateTime startDate, DateTime endDate)
        {
            EventId = eventId;
            EventName = eventName;
            OrganizerId = organizerId;
            OrganizerType = organizerType;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
