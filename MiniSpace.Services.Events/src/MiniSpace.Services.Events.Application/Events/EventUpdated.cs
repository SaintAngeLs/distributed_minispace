using System;
using System.Collections;
using System.Collections.Generic;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventUpdated(Guid eventId, DateTime updatedAt, Guid updatedBy, OrganizerType organizerType, IEnumerable<string> mediaFilesUrls) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public DateTime UpdatedAt { get; set; } = updatedAt;
        public Guid UpdatedBy { get; set; } = updatedBy;
        public OrganizerType OrganizerType { get; set; } = organizerType; 
        public IEnumerable<string> MediaFilesUrls { get; set; } = mediaFilesUrls; 
    }
}
