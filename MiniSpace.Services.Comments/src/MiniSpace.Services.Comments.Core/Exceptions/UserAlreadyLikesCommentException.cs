using System;

namespace MiniSpace.Services.Comments.Core.Exceptions
{
    public class UserAlreadyLikesCommentException : DomainException
    {
        public override string Code { get; } = "user_already_likes_comment";
        public Guid UserId { get; }
        
        public UserAlreadyLikesCommentException(Guid userId) 
            : base($"User with id: {userId} already likes this comment.")
        {
            UserId = userId;
        }
    }
}
