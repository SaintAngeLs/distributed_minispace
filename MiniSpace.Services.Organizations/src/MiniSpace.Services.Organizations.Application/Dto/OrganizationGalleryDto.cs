using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<GalleryImageDto> Gallery { get; set; }

        public OrganizationGalleryDto(Organization organization, IEnumerable<GalleryImage> gallery)
        {
            Organization = new OrganizationDto(organization);
            Gallery = gallery.Select(g => new GalleryImageDto(g)).ToList();
        }
    }
    
}
