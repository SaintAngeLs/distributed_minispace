
namespace MiniSpace.Services.Organizations.Core.Events
{
    public class RoleAssignedToUser : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public string Role { get; }

        public RoleAssignedToUser(Guid organizationId, Guid userId, string role)
        {
            OrganizationId = organizationId;
            UserId = userId;
            Role = role;
        }
    }
}