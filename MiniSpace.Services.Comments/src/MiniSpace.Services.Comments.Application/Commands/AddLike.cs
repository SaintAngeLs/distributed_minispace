using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class AddLike : ICommand
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }

        public AddLike(Guid commentId, Guid userId, string commentContext)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
        }
    }
}
