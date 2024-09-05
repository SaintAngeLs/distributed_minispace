using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class UserIsNotBannedException : DomainException
    {
        public override string Code { get; } = "user_is_not_banned";
        public Guid UserId { get; }
        public string Role { get; }

        public UserIsNotBannedException(Guid userId, string role) : 
            base($"User with ID: {userId} and Role: {role} is not banned.")
        {
            UserId = userId;
            Role = role;
        }
    }
}