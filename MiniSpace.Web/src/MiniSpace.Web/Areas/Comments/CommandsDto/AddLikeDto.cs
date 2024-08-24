using System;

namespace MiniSpace.Web.Areas.Comments.CommandDto
{
    public class AddLikeDto
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }

        public AddLikeDto(Guid commentId, Guid userId, string commentContext)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
        }
    }
}
