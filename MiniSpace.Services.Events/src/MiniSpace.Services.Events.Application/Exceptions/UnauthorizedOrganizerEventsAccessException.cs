using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UnauthorizedOrganizerEventsAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_event_access";
        public Guid OrganizerId { get; }
        public Guid UserId { get; }

        public UnauthorizedOrganizerEventsAccessException(Guid organizerId, Guid userId) 
            : base($"Unauthorized access to organizer events with ID: '{organizerId}' by user with ID: '{userId}'.")
        {
            OrganizerId = organizerId;
            UserId = userId;
        }
    }
}