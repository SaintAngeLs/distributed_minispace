using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class EventNotPublishedException : DomainException
    {
        public override string Code { get; } = "event_not_published";
        public Guid EventId { get; }
        public EventNotPublishedException(Guid eventId) : base($"Event with id: {eventId} is not published.")
        {
            EventId = eventId;
        }
    }
}