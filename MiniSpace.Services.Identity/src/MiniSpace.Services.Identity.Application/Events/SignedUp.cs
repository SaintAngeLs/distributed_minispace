using System;
using Paralax.CQRS.Events;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.Events
{
    
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Role { get; }
        public string Token { get; }
        public string HashedToken { get; }

        public SignedUp(Guid userId, string firstName, string lastName, string email, string role, string token, string hashedToken)
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
