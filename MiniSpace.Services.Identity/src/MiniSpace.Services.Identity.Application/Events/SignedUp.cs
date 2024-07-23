using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.Events
{
    [Contract]
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public Role Role { get; }
        public string Token { get; }
        public string HashedToken { get; }

        public SignedUp(Guid userId, string firstName, string lastName, string email, Role role, string token, string hashedToken)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            Token = token;
            HashedToken = hashedToken;
        }
    }
}
