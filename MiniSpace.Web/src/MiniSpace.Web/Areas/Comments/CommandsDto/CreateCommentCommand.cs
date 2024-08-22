using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Comments.CommandsDto
{
    public class CreateCommentCommand
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }

        public CreateCommentCommand(Guid commentId, Guid contextId, string commentContext, Guid studentId, Guid parentId, string comment)
        {
            CommentId = commentId;
            ContextId = contextId;
            CommentContext = commentContext;
            StudentId = studentId;
            ParentId = parentId;
            Comment = comment;
        }
    }
}