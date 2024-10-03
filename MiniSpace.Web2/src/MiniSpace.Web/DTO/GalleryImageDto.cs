using System;

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
            ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? imageUrl : "/images/default_image.png";  
            DateAdded = dateAdded;
        }
    }
}
