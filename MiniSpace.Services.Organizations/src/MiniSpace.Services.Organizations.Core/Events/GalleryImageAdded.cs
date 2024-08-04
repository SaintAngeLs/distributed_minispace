using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class GalleryImageAdded : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid ImageId { get; }
        public string Url { get; }
        public DateTime CreatedAt { get; }

        public GalleryImageAdded(Guid organizationId, Guid imageId, string url, DateTime createdAt)
        {
            OrganizationId = organizationId;
            ImageId = imageId;
            Url = url;
            CreatedAt = createdAt;
        }
    }
}
