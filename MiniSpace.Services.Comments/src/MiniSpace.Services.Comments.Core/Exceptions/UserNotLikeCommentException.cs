using System;

namespace MiniSpace.Services.Comments.Core.Exceptions
{
    public class UserNotLikeCommentException : DomainException
    {
        public override string Code { get; } = "user_not_liked_comment";
        public Guid UserId { get; }
        public Guid CommentId { get; }
        
        public UserNotLikeCommentException(Guid userId, Guid commentId) 
            : base($"User with id: {userId} has not liked the comment with id: {commentId}.")
        {
            UserId = userId;
            CommentId = commentId;
        }
    }
}
