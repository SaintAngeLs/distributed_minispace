using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Comments
{
    [Message("comments")]
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }
        public DateTime UpdatedAt { get; }
        public string CommentContent { get; }
        public string UserName { get; }  
        public string ProfileImageUrl { get; } 

        public CommentUpdated(Guid commentId, Guid userId, string commentContext, 
        DateTime updatedAt, string commentContent, string userName, string profileImageUrl)
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
