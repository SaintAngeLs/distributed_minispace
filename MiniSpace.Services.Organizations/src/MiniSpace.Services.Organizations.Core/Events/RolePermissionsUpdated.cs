using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RolePermissionsUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public Dictionary<Permission, bool> Permissions { get; }

        public RolePermissionsUpdated(Guid organizationId, Guid roleId, Dictionary<Permission, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            Permissions = permissions;
        }
    }
}