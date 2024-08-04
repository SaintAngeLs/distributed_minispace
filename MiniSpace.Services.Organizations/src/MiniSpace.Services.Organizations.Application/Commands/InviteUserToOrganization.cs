using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class InviteUserToOrganization : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }

        public InviteUserToOrganization(Guid organizationId, Guid userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }
    }
}
