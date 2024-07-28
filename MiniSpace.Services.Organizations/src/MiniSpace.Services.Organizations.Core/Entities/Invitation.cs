namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Invitation
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }

        public Invitation(Guid organizationId, Guid userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }
    }
}
