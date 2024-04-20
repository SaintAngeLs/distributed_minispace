using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class AuthorizedUserIsNotAnOrganizerException: AppException
    {
        public Guid UserId { get; }
        
        public AuthorizedUserIsNotAnOrganizerException(Guid userId)
            : base($"User with id: '{userId}' is not an organizer")
        {
            UserId = userId;
        }
    }
}