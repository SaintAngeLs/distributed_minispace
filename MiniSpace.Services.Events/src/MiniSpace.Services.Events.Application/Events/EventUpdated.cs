using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventUpdated(Guid eventId, DateTime updatedAt, Guid updatedBy, IEnumerable<Guid> mediaFilesIds) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
        public DateTime UpdatedAt { get; set; } = updatedAt;
        public Guid UpdatedBy { get; set; } = updatedBy;
        public IEnumerable<Guid> MediaFilesIds { get; set; } = mediaFilesIds;
    }
}