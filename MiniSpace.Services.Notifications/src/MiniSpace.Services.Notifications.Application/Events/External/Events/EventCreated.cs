using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events
{
    [Message("events")]
    public class EventCreated : IEvent
    {
        public Guid EventId { get; set; }
        /* OrganizerType := { User, Organization} */
        public string OrganizerType { get; set; }  
        public Guid OrganizerId { get; set; }
        public IEnumerable<string> MediaFilesUrls { get; set; }

        public EventCreated(Guid eventId, string organizerType, Guid organizerId, IEnumerable<string> mediaFilesUrls)
        {
            EventId = eventId;
            OrganizerType = organizerType;
            OrganizerId = organizerId;
            MediaFilesUrls = mediaFilesUrls;
        }
    }
}
