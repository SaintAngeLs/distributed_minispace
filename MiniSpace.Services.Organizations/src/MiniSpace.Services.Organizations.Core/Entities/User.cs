using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class User
    {
        public Guid Id { get; }
        public Guid OrganizationId { get; }
        public Role Role { get; private set; }

        public User(Guid id, Guid organizationId, Role role)
        {
            Id = id;
            OrganizationId = organizationId;
            Role = role;
        }

        public bool HasPermission(Permission permission)
        {
            return Role.Permissions.ContainsKey(permission) && Role.Permissions[permission];
        }

        public void AssignRole(Role role)
        {
            Role = role;
        }
    }
}
