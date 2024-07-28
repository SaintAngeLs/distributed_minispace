using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
        public IEnumerable<UserDto> Users { get; set; }

        public OrganizationGalleryUsersDto(Organization organization, IEnumerable<GalleryImage> gallery, IEnumerable<User> users)
        {
            Organization = new OrganizationDto(organization);
            Gallery = gallery.Select(g => new GalleryImageDto(g)).ToList();
            Users = users.Select(u => new UserDto(u)).ToList();
        }
    }
}
