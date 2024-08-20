using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class UpdateComment : ICommand
    {
        public Guid CommentId { get; }
        public string TextContent { get; }
        public string CommentContext { get; }

        public UpdateComment(Guid commentId, string textContent, string commentContext)
        {
            CommentId = commentId;
            TextContent = textContent;
            CommentContext = commentContext;
        }
    }    
}
