using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RoleUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string NewName { get; }
        public string NewDescription { get; }
        public Dictionary<Permission, bool> NewPermissions { get; }

        public RoleUpdated(Guid organizationId, Guid roleId, string newName, string newDescription, Dictionary<Permission, bool> newPermissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            NewName = newName;
            NewDescription = newDescription;
            NewPermissions = newPermissions;
        }
    }
}
