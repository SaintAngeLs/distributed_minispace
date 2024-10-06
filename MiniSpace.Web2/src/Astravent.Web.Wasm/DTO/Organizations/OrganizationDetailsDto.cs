using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Organizations
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
        public IEnumerable<SubOrganizationDto> SubOrganizations { get; set; } = new List<SubOrganizationDto>();
        public IEnumerable<InvitationDto> Invitations { get; set; } = new List<InvitationDto>();
        public IEnumerable<OrganizationUserDto> Users { get; set; } = new List<OrganizationUserDto>();
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public IEnumerable<GalleryImageDto> Gallery { get; set; } = new List<GalleryImageDto>();
        public OrganizationSettingsDto Settings { get; set; }
        public string DefaultRoleName { get; set; }

        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

    }
}