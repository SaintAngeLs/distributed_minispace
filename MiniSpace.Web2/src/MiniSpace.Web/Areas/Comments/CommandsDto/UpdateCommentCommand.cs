using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Comments.CommandsDto
{
    public class UpdateCommentCommand
    {
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }

        public UpdateCommentCommand(Guid commentId, string textContent)
        {
            CommentId = commentId;
            TextContent = textContent;
        }
    }
}