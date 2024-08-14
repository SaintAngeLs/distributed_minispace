using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class UserPostAddedEvent : IDomainEvent
    {
        public Guid UserPostId { get; }
        public Guid PostId { get; }

        public UserPostAddedEvent(Guid userPostId, Guid postId)
        {
            UserPostId = userPostId;
            PostId = postId;
        }
    }
}
