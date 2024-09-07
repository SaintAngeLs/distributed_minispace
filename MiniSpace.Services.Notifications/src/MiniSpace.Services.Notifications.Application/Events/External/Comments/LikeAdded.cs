using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Comments
{
    [Message("comments")]
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
