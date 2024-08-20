using System;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class PostVisibilityChangedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public VisibilityStatus Visibility { get; }
        public DateTime ChangedAt { get; }

        public PostVisibilityChangedEvent(Guid postId, VisibilityStatus visibility, DateTime changedAt)
        {
            PostId = postId;
            Visibility = visibility;
            ChangedAt = changedAt;
        }
    }
}
