using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class GalleryImageRemoved : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid ImageId { get; }
        public DateTime RemovedAt { get; }

        public GalleryImageRemoved(Guid organizationId, Guid imageId, DateTime removedAt)
        {
            OrganizationId = organizationId;
            ImageId = imageId;
            RemovedAt = removedAt;
        }
    }
}
