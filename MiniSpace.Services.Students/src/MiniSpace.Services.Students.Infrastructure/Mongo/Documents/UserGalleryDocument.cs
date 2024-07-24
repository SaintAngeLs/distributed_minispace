using Convey.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserGalleryDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<GalleryImageDocument> GalleryOfImages { get; set; }

        public UserGalleryDocument()
        {
            GalleryOfImages = new List<GalleryImageDocument>();
        }
    }

    public class GalleryImageDocument
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public GalleryImageDocument(Guid imageId, string imageUrl)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            CreatedAt = DateTime.UtcNow;
        }
    }
}
