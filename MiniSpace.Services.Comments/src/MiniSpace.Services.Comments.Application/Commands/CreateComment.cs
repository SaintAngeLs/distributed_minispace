using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class CreateComment : ICommand
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; } 
        public Guid UserId { get; set; }
        public Guid ParentId { get; set; }
        public string TextContent { get; set; } 

        public CreateComment(Guid commentId, Guid contextId, string commentContext, Guid userId, Guid parentId,
            string textContent)
        {
            CommentId = commentId == Guid.Empty ? Guid.NewGuid() : commentId;
            ContextId = contextId;
            CommentContext = commentContext;
            UserId = userId;
            ParentId = parentId;
            TextContent = textContent;
        }
    }
}
