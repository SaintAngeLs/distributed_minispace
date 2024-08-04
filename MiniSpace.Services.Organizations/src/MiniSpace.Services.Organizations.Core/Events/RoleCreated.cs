using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RoleCreated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string RoleName { get; }
        public string Description { get; }
        public Dictionary<Permission, bool> Permissions { get; }

        public RoleCreated(Guid organizationId, Guid roleId, string roleName, string description, Dictionary<Permission, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            Permissions = permissions;
        }
    }
}
