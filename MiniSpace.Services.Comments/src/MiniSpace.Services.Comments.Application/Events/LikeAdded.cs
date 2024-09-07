using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class LikeAdded : IEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }
        public DateTime LikedAt { get; }

        public LikeAdded(Guid commentId, Guid userId, string commentContext, DateTime likedAt)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
            LikedAt = likedAt;
        }
    }
}
