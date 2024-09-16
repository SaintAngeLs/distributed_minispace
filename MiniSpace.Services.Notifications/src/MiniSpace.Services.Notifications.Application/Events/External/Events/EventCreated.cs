using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("events")]
    public class EventCreated : IEvent
    {
        public Guid EventId { get; set; }
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
