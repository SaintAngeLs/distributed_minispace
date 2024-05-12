using System;
using System.Collections.Generic;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventDeleted(Guid eventId, IEnumerable<Guid> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public IEnumerable<Guid> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}