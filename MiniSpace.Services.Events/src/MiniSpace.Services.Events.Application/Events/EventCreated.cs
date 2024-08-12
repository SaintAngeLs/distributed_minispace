using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventCreated(Guid eventId, OrganizerType organizerType, Guid organizerId, IEnumerable<string> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public OrganizerType OrganizerType { get; set; } = organizerType;
        public Guid OrganizerId { get; set; } = organizerId;
        public IEnumerable<string> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}
