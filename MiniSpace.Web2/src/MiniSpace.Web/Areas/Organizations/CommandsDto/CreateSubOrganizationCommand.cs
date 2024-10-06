using System;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class CreateSubOrganizationCommand
    {
         public Guid SubOrganizationId { get; }
        public string Name { get; }
        public string Description { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public Guid OwnerId { get; }
        public OrganizationSettingsDto Settings { get; }
        public string BannerUrl { get; }
        public string ImageUrl { get; }
    }
}
