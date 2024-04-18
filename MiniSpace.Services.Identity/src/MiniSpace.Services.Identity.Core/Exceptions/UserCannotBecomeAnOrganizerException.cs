using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class UserCannotBecomeAnOrganizerException : DomainException
    {
        public override string Code { get; } = "user_cannot_become_an_organizer";
        public Guid UserId { get; }
        public string Role { get; }

        public UserCannotBecomeAnOrganizerException(Guid userId, string role) : base($"User with ID: {userId} and role: {role} is cannot become an organizer.")
        {
            UserId = userId;
            Role = role;
        }
    }
}