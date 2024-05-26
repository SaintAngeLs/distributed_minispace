using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("events")]
    public class EventDeleted(Guid eventId) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
    }
}