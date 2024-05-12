using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class InvalidParentCommentException : AppException
    {
        public override string Code { get; } = "invalid_parent_comment";
        public Guid ParentId { get; }

        public InvalidParentCommentException(Guid parentId) : base($"Invalid parent comment with id: '{parentId}'. It cannot be a child comment.")
        {
            ParentId = parentId;
        }
    }
}