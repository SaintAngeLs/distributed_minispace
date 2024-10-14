using System;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
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
