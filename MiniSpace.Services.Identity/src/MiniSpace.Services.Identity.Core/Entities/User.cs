using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Identity.Core.Exceptions;

namespace MiniSpace.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IEnumerable<string> Permissions { get; private set; }

        public User(Guid id, string name, string email, string password, string role, DateTime createdAt,
            IEnumerable<string> permissions = null)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException(name);
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (!Entities.Role.IsValid(role))
            {
                throw new InvalidRoleException(role);
            }

            Id = id;
            Name = name;
            Email = email.ToLowerInvariant();
            Password = password;
            Role = role.ToLowerInvariant();
            CreatedAt = createdAt;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }
        
        public void GrantOrganizerRights()
        {
            if (Permissions.Contains(UserPermissions.OrganizeEvents))
            {
                throw new UserAlreadyAnOrganizerException(Id);
            }
            Permissions = Permissions.Append(UserPermissions.OrganizeEvents);
        }
        
        public void RevokeOrganizerRights()
        {
            if (!Permissions.Contains(UserPermissions.OrganizeEvents))
            {
                throw new UserIsNotAnOrganizerException(Id);
            }

            var permissionsList = Permissions.ToList();
            permissionsList.Remove(UserPermissions.OrganizeEvents);
            Permissions = permissionsList;
        }
    }

    public static class UserPermissions
    {
        public static string OrganizeEvents { get; private set; } = "organize_events";
    }
}