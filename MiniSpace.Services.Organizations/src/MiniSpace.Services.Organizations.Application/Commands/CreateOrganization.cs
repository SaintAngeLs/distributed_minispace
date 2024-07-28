using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateOrganization : ICommand
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public OrganizationSettings Settings { get; }

        public CreateOrganization(Guid organizationId, string name, Guid rootId, Guid parentId, OrganizationSettings settings)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
            Name = name;
            RootId = rootId;
            ParentId = parentId;
            Settings = settings;
        }
    }
}
