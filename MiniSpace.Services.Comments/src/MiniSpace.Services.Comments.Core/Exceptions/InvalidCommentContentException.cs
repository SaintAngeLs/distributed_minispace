using System;

namespace MiniSpace.Services.Comments.Core.Exceptions
{
    public class InvalidCommentContentException : DomainException
    {
        public override string Code { get; } = "invalid_comment_content";
        public Guid Id { get; }
        
        public InvalidCommentContentException(Guid id) : base(
            $"Comment with id: {id} has invalid content.")
        {
            Id = id;
        }
    }
}
