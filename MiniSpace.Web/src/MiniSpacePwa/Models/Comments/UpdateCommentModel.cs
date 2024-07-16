using System;

namespace MiniSpacePwa.Models.Comments
{
    public class UpdateCommentModel
    {
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }
        public bool UpdatingSubmitted { get; set; }
    }
}
