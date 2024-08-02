using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class UpdateOrganizationCommand
    {
        public Guid OrganizationId { get; private set; }
        public string Name { get; }
        public string Description { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public Guid OwnerId { get; }
        public OrganizationSettingsDto Settings { get; }
        public string BannerUrl { get; }
        public string ImageUrl { get; }

        public UpdateOrganizationCommand(Guid organizationId, string name, string description, Guid rootId, Guid parentId, Guid ownerId, OrganizationSettingsDto settings, string bannerUrl, string imageUrl)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
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
