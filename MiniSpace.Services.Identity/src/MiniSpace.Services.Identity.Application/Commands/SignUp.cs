using System;
using System.Collections.Generic;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class SignUp : ICommand
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }
        public IEnumerable<string> Permissions { get; }

        public SignUp(Guid userId, string firstName, string lastName, string email, string password, string role,
            IEnumerable<string> permissions)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            Permissions = permissions;
        }

    }
}