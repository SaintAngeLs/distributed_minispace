using System;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class EventNotFoundException : AppException
    {
        public override string Code { get; } = "event_not_found";
        public Guid EventId { get; }

        public EventNotFoundException(Guid eventId) : base($"Event with ID: '{eventId}' was not found.")
        {
            EventId = eventId;
        }
    }
}