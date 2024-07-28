using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class User
    {
        public Guid Id { get; }
        private ISet<Role> _roles = new HashSet<Role>();

        public IEnumerable<Role> Roles
        {
            get => _roles;
            private set => _roles = new HashSet<Role>(value);
        }

        public User(Guid id)
        {
            Id = id;
        }

        public bool HasPermission(Permission permission)
        {
            return _roles.Any(role => role.Permissions.ContainsKey(permission) && role.Permissions[permission]);
        }

        public void AssignRole(Role role)
        {
            _roles.Add(role);
        }
    }
}
