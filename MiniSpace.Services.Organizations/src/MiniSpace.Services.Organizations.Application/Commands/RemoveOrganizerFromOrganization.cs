using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class RemoveOrganizerFromOrganization : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid OrganizerId { get; }

        public RemoveOrganizerFromOrganization(Guid organizationId, Guid organizerId)
        {
            OrganizationId = organizationId;
            OrganizerId = organizerId;
        }
    }
}