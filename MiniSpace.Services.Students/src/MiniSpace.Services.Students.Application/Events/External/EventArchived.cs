using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class EventArchived : IEvent
    {
        public Guid EventId { get; }

        public EventArchived(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
