using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class RemoveOrganizerFromOrganization : ICommand
    {
        public Guid OrganizationId { get; set; }
        public Guid OrganizerId { get; set; }

        public RemoveOrganizerFromOrganization(Guid organizationId, Guid organizerId)
        {
            OrganizationId = organizationId;
            OrganizerId = organizerId;
        }
    }
}