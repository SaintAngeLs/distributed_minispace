using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class UnauthorizedCommentAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_comment_access";
        public Guid CommentId { get; }
        public Guid UserId { get; }

        public UnauthorizedCommentAccessException(Guid commentId, Guid userId)
            : base($"Unauthorized access to comment with id: '{commentId}' by user with id: '{userId}'.")
        {
            CommentId = commentId;
            UserId = userId;
        }
    }
}
