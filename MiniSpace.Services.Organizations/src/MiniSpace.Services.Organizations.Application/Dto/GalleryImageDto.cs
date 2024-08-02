using System;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class GalleryImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public GalleryImageDto()
        {
            
        }

        public GalleryImageDto(GalleryImage galleryImage)
        {
            Id = galleryImage.Id;
            Url = galleryImage.Url;
            Description = galleryImage.Description;
            CreatedAt = galleryImage.CreatedAt;
        }
    }
}
