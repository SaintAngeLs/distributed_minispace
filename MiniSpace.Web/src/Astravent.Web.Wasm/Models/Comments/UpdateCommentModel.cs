using System;

namespace Astravent.Web.Wasm.Models.Comments
{
    public class UpdateCommentModel
    {
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }
        public bool UpdatingSubmitted { get; set; }
    }
}
