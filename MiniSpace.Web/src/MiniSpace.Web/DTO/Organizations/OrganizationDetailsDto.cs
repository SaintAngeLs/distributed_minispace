using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? ParentOrganizationId { get; set; }
        public IEnumerable<SubOrganizationDto> SubOrganizations { get; set; }
        public IEnumerable<InvitationDto> Invitations { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
        public OrganizationSettingsDto Settings { get; set; }
    }
}