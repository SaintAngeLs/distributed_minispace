using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class OrganizationPostRemovedEvent : IDomainEvent
    {
        public Guid OrganizationPostId { get; }
        public Guid PostId { get; }

        public OrganizationPostRemovedEvent(Guid organizationPostId, Guid postId)
        {
            OrganizationPostId = organizationPostId;
            PostId = postId;
        }
    }
}
