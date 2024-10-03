using System;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class PostRepostedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public Guid UserId { get; }
        public Guid OriginalPostId { get; }
        public DateTime RepostedAt { get; }

        public PostRepostedEvent(Guid postId, Guid userId, Guid originalPostId, DateTime repostedAt)
        {
            PostId = postId;
            UserId = userId;
            OriginalPostId = originalPostId;
            RepostedAt = repostedAt;
        }
    }
}
