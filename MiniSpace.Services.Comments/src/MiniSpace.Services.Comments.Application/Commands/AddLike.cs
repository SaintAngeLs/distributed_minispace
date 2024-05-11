using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class AddLike : ICommand
    {
        public Guid CommentId { get; set; }

        public AddLike(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
