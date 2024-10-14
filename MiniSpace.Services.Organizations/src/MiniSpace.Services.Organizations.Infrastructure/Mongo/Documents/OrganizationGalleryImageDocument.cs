using Paralax.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryImageDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<GalleryImageEntry> Gallery { get; set; }
    }

    public class GalleryImageEntry
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
