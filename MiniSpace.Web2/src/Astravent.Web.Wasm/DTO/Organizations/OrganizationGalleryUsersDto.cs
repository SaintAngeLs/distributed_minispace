using System.Collections.Generic;

using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Organizations
{
    public class OrganizationGalleryUsersDto
    {
        public OrganizationDetailsDto OrganizationDetails { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; } = new List<GalleryImageDto>();
        public IEnumerable<OrganizationUserDto> Users { get; set; } = new List<OrganizationUserDto>();
    }
}
