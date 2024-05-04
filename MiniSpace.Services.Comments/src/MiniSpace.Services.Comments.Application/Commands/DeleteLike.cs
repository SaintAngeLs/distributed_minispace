using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class DeleteLike : ICommand
    {
        public Guid CommentId { get; }

        public DeleteLike(Guid commentId) => CommentId = commentId;
    }    
}
