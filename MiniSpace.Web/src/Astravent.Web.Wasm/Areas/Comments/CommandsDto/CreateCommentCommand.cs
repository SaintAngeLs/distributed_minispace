using System;

namespace Astravent.Web.Wasm.Areas.Comments.CommandsDto
{
    public class CreateCommentCommand
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        // CommentsContext := UserPost || UserEvent || OrganizationPost || OrganizationEvent
        public Guid UserId { get; set; } 
        public Guid ParentId { get; set; }
        public string TextContent { get; set; }

        public CreateCommentCommand(Guid commentId, Guid contextId, string commentContext, Guid userId, Guid parentId, string textContent)
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
