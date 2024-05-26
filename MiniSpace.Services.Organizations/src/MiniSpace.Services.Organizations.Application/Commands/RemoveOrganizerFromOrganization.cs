using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class RemoveOrganizerFromOrganization : ICommand
    {
        public Guid RootOrganizationId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid OrganizerId { get; set; }

        public RemoveOrganizerFromOrganization(Guid rootOrganizationId, Guid organizationId, Guid organizerId)
        {
            RootOrganizationId = rootOrganizationId;
            OrganizationId = organizationId;
            OrganizerId = organizerId;
        }
    }
}