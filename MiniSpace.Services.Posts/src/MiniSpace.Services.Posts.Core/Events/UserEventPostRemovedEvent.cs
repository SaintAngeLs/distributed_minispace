using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class UserEventPostRemovedEvent : IDomainEvent
    {
        public Guid UserEventPostId { get; }
        public Guid PostId { get; }

        public UserEventPostRemovedEvent(Guid userEventPostId, Guid postId)
        {
            UserEventPostId = userEventPostId;
            PostId = postId;
        }
    }
}
