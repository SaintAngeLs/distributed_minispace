using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateOrganization: ICommand
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }

        public CreateOrganization(Guid organizationId, string name, Guid rootId, Guid parentId)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
            Name = name;
            RootId = rootId;
            ParentId = parentId;
        }
    }
}

