using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class UnauthorizedEventAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_event_access";
        public Guid EventId { get; }
        public Guid UserId { get; }

        public UnauthorizedEventAccessException(Guid eventId, Guid userId) 
            : base($"Unauthorized access to event with ID: '{eventId}' by user with ID: '{userId}'.")
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}