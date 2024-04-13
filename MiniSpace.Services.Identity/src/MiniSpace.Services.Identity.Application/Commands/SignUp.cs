using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    [Contract]
    public class SignUp : ICommand
    {
        public Guid UserId { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }
        public IEnumerable<string> Permissions { get; }

        public SignUp(Guid userId, string name, string email, string password, string role, IEnumerable<string> permissions)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            Permissions = permissions;
        }
    }
}