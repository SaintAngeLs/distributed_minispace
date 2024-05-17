using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateRootOrganization: ICommand
    {
        public Guid OrganizationId { get; }
        public string Name { get; }

        public CreateRootOrganization(Guid organizationId, string name)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
            Name = name;
        }
    }
}