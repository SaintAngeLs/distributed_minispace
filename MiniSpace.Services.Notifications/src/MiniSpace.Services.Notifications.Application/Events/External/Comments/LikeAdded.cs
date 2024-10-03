using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
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
        public string UserName { get; }       
        public string ProfileImageUrl { get; }  

        public LikeAdded(Guid commentId, Guid userId, string commentContext, 
        DateTime likedAt, string userName, string profileImageUrl)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
            LikedAt = likedAt;
            UserName = userName;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
