using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class ParentCommentNotFoundException : AppException
    {
        public override string Code { get; } = "parent_comment_not_found";
        public Guid ParentId { get; }

        public ParentCommentNotFoundException(Guid parentId) : base($"Parent comment with id: '{parentId}' was not found.")
        {
            ParentId = parentId;
        }
    }
}