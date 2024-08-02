using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Organizations
{
    public class OrganizationGalleryUsersDto
    {
        public OrganizationDetailsDto Organization { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
