using System;

namespace Astravent.Web.Wasm.Models.Comments
{
    public class DeleteCommentModel
    {
        public Guid CommentId { get; set; }
        public bool DeletingSubmitted { get; set; }
    }
}
