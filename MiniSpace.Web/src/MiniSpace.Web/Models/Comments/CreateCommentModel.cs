using System;

namespace MiniSpace.Web.Models.Comments
{
    public class CreateCommentModel
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
    }
}
