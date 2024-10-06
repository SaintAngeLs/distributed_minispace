using Paralax.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateSubOrganization : ICommand
    {
        public Guid SubOrganizationId { get; }
        public string Name { get; }
        public string Description { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public Guid OwnerId { get; }
        public OrganizationSettings Settings { get; }
        public string BannerUrl { get; }
        public string ImageUrl { get; }

        public CreateSubOrganization(Guid subOrganizationId, string name, string description, Guid rootId, Guid parentId, Guid ownerId, OrganizationSettings settings, string bannerUrl, string imageUrl)
        {
            SubOrganizationId = subOrganizationId == Guid.Empty ? Guid.NewGuid() : subOrganizationId;
            Name = name;
            Description = description;
            RootId = rootId;
            ParentId = parentId;
            OwnerId = ownerId;
            Settings = settings;
            BannerUrl = bannerUrl;
            ImageUrl = imageUrl;
        }
    }
}
