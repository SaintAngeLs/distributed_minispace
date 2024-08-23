using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class UserPostRemovedEvent : IDomainEvent
    {
        public Guid UserPostId { get; }
        public Guid PostId { get; }

        public UserPostRemovedEvent(Guid userPostId, Guid postId)
        {
            UserPostId = userPostId;
            PostId = postId;
        }
    }
}
