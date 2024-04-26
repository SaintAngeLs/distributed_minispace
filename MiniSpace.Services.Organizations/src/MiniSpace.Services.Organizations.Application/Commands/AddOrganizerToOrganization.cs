using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AddOrganizerToOrganization: ICommand
    {
        public Guid OrganizationId { get; set; }
        public Guid OrganizerId { get; set; }
        
        public AddOrganizerToOrganization(Guid organizationId, Guid organizerId)
        {
            OrganizationId = organizationId;
            OrganizerId = organizerId;
        }
    }
}