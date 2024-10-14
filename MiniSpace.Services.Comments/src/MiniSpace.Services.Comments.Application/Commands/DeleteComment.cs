using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class DeleteComment : ICommand
    {
        public Guid CommentId { get; }
        public string CommentContext { get; }
        
        public DeleteComment(Guid commentId, string commentContext)
        {
            CommentId = commentId;
            CommentContext = commentContext;
        }
    }
}
