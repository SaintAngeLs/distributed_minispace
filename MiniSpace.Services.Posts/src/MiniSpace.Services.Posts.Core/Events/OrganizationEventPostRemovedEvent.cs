using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class OrganizationEventPostRemovedEvent : IDomainEvent
    {
        public Guid OrganizationEventPostId { get; }
        public Guid PostId { get; }

        public OrganizationEventPostRemovedEvent(Guid organizationEventPostId, Guid postId)
        {
            OrganizationEventPostId = organizationEventPostId;
            PostId = postId;
        }
    }
}
