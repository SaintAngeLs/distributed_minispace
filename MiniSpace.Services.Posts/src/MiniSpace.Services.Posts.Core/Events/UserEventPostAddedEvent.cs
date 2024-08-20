using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class UserEventPostAddedEvent : IDomainEvent
    {
        public Guid UserEventPostId { get; }
        public Guid PostId { get; }

        public UserEventPostAddedEvent(Guid userEventPostId, Guid postId)
        {
            UserEventPostId = userEventPostId;
            PostId = postId;
        }
    }
}
