namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RoleNameUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string NewName { get; }

        public RoleNameUpdated(Guid organizationId, Guid roleId, string newName)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            NewName = newName;
        }
    }
}
