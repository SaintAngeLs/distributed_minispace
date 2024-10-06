using System;

namespace MiniSpace.Web.Models.Comments
{
    public class DeleteCommentModel
    {
        public Guid CommentId { get; set; }
        public bool DeletingSubmitted { get; set; }
    }
}
