using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Comments.Application.Events.External
{
    [Message("events")]
    public class EventDeleted : IEvent
    {
        public Guid EventId { get; }

        public EventDeleted(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
