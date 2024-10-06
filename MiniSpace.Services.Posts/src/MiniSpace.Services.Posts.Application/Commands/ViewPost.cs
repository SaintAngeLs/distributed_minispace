using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class ViewPost : ICommand
    {
        public Guid UserId { get; }
        public Guid PostId { get; }

        public ViewPost(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId = postId;
        }
    }
}
