using System;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class CommentNotFoundException : AppException
    {
        public override string Code { get; } = "comment_not_found";
        public Guid CommentId { get; }

        public CommentNotFoundException(Guid commentId) : 
                    base($"Comment with id: {commentId} was not found.")
        {
            CommentId = commentId;
        }
    }
}
