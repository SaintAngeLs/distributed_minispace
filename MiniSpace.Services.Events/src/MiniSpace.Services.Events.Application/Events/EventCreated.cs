eusing System;
using System.Collections.Generic;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventCreated(Guid eventId, Guid organizerId, IEnumerable<Guid> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public Guid OrganizerId { get; set; } = organizerId;
        public IEnumerable<Guid> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}