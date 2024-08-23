using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class OrganizationEventPostAddedEvent : IDomainEvent
    {
        public Guid OrganizationEventPostId { get; }
        public Guid PostId { get; }

        public OrganizationEventPostAddedEvent(Guid organizationEventPostId, Guid postId)
        {
            OrganizationEventPostId = organizationEventPostId;
            PostId = postId;
        }
    }
}
