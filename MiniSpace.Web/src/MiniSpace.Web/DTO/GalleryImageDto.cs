using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Web.DTO
{
    public class GalleryImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public GalleryImageDto(Guid imageId, string imageUrl, DateTime dateAdded)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            DateAdded = dateAdded;
        }
    }
}
