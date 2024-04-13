using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class UserAlreadyAnOrganizerException : DomainException
    {
        public override string Code { get; } = "user_already_an_organizer";
        public Guid UserId { get; }

        public UserAlreadyAnOrganizerException(Guid userId) : base($"User with ID: {userId} is already an organizer.")
        {
            UserId = userId;
        }
    }
}