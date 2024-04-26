using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AddOrganizerToOrganization: ICommand
    {
        public Guid OrganizerId { get; set; }
        public Guid OrganizationId { get; set; }
    }
}