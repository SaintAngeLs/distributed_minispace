using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class UserIsNotAnOrganizerException : DomainException
    {
        public override string Code { get; } = "user_is_not_an_organizer";
        public Guid UserId { get; }

        public UserIsNotAnOrganizerException(Guid userId) : base($"User with ID: {userId} is not an organizer.")
        {
            UserId = userId;
        }
    }
}