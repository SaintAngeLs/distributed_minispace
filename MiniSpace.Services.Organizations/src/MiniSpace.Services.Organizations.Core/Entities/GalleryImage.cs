using System;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class GalleryImage
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public GalleryImage(Guid id, string url, DateTime createdAt, string description = null)
        {
            Id = id;
            Url = url;
            CreatedAt = createdAt;
            Description = description;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
