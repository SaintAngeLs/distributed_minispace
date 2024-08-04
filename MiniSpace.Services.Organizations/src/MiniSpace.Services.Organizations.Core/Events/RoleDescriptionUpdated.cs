namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RoleDescriptionUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string NewDescription { get; }

        public RoleDescriptionUpdated(Guid organizationId, Guid roleId, string newDescription)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            NewDescription = newDescription;
        }
    }
}
