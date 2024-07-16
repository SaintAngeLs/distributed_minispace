using System;

namespace MiniSpacePwa.Models.Comments
{
    public class CreateCommentModel
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
        public bool CreatingSubmitted { get; set; }
    }
}
