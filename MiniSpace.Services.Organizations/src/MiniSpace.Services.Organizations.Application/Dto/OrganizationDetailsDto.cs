using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public IEnumerable<SubOrganizationDto> SubOrganizations { get; set; }
        public IEnumerable<InvitationDto> Invitations { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
        public OrganizationSettingsDto Settings { get; set; }

        public OrganizationDetailsDto()
        {
            
        }

        public OrganizationDetailsDto(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Description = organization.Description;
            BannerUrl = organization.BannerUrl;
            ImageUrl = organization.ImageUrl;
            OwnerId = organization.OwnerId;
            SubOrganizations = organization.SubOrganizations.Select(o => new SubOrganizationDto(o)).ToList();
            Invitations = organization.Invitations.Select(i => new InvitationDto(i)).ToList();
            Users = organization.Users.Select(u => new UserDto(u)).ToList();
            Roles = organization.Roles.Select(r => new RoleDto(r)).ToList();
            Gallery = organization.Gallery.Select(g => new GalleryImageDto(g)).ToList();
            Settings = new OrganizationSettingsDto(organization.Settings);
        }
    }
}
