using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Web.DTO.Organizations
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }
    }
    
}
