using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class GalleryImage
    {
        public Guid ImageId { get; private set; }
        public string ImageUrl { get; private set; }
        public DateTime DateAdded { get; private set; }

        public GalleryImage(Guid imageId, string imageUrl, DateTime dateAdded)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            DateAdded = dateAdded;
        }
    }
}
