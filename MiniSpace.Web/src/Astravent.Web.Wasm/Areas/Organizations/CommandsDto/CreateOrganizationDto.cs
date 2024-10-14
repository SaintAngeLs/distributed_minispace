using System;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class CreateOrganizationDto
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? RootId { get; set; } 
        public Guid? ParentId { get; set; } = null; 
        public Guid OwnerId { get; set; }
        public OrganizationSettingsDto Settings { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
