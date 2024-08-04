using System;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class GalleryImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public GalleryImageDto()
        {
            
        }

        public GalleryImageDto(GalleryImage galleryImage)
        {
            ImageId = galleryImage.ImageId;
            ImageUrl = galleryImage.ImageUrl;
            DateAdded = galleryImage.DateAdded;
        }

        public GalleryImageDto(Guid imageId, string imageUrl, DateTime dateAdded)
        {
            ImageId = imageId;
            ImageUrl = imageUrl;
            DateAdded = dateAdded;
        }
    }
}
