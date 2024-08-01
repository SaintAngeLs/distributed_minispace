using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Web.DTO;

namespace MiniSpace.Web.DTO.Organizations
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
