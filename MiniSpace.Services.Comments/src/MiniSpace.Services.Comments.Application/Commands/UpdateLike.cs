using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class UpdateLike : ICommand
    {
        public Guid CommentId { get; set; }

        public UpdateLike(Guid id)
        {
            CommentId = id;
        }
    }
}
