using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventIdException : AppException
    {
        public override string Code { get; } = "invalid_event_id";
        public Guid EventId { get; }

        public InvalidEventIdException(Guid eventId) : base($"Invalid event id: {eventId}.")
        {
            EventId = eventId;
        }
    }
}