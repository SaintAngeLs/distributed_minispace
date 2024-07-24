using Convey.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class GalleryImageDocument
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public GalleryImageDocument(Guid imageId, string imageUrl)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            DateAdded = DateTime.UtcNow;
        }
    }
}
