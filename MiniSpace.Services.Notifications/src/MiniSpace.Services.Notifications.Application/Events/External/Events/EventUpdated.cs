using System;
using System.Collections;
using System.Collections.Generic;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.Events
{
    [Contract]
    public class EventUpdated(Guid eventId, DateTime updatedAt, Guid updatedBy, IEnumerable<Guid> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public DateTime UpdatedAt { get; set; } = updatedAt;
        public Guid UpdatedBy { get; set; } = updatedBy;
        public IEnumerable<Guid> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}