using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }
        public DateTime UpdatedAt { get; }
        public string CommentContent { get; }
        public string UserName { get; }
        public string ProfileImageUrl { get; }

        public CommentUpdated(Guid commentId, Guid userId, string commentContext, DateTime updatedAt, string commentContent, string userName, string profileImageUrl)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
            UpdatedAt = updatedAt;
            CommentContent = commentContent;
            UserName = userName;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
