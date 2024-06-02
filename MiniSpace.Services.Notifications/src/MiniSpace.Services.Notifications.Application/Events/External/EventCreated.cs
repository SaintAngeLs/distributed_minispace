using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("events")]
    public class EventCreated(Guid eventId, Guid organizerId, IEnumerable<Guid> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public Guid OrganizerId { get; set; } = organizerId;
        public IEnumerable<Guid> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}