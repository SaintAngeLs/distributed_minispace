using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AddOrganization: ICommand
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public Guid ParentId { get; }

        public AddOrganization(Guid organizationId, string name, Guid parentId)
        {
            OrganizationId = organizationId;
            Name = name;
            ParentId = parentId;
        }
    }
}

