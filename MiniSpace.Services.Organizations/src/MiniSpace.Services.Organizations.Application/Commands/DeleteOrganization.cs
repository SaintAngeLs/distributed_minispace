using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class DeleteOrganization: ICommand
    {
        public Guid OrganizationId { get; set; }
        public Guid RootId { get; }

        public DeleteOrganization(Guid organizationId, Guid rootId)
        {
            OrganizationId = organizationId;
            RootId = rootId;
        }
    }
}