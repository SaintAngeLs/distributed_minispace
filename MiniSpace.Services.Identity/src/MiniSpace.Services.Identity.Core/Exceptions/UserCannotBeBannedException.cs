using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class UserCannotBeBannedException : DomainException
    {
        public override string Code { get; } = "user_cannot_be_banned";
        public Guid UserId { get; }
        public string Role { get; }

        public UserCannotBeBannedException(Guid userId, string role) : 
            base($"User with ID: {userId} and Role: {role} cannot be banned.")
        {
            UserId = userId;
            Role = role;
        }
    }
}