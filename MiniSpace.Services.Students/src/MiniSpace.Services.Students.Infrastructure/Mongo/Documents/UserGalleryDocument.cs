using Paralax.Types;
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
}
