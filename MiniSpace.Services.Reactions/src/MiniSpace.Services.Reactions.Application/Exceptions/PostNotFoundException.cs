using System;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class PostNotFoundException : AppException
    {
        public override string Code { get; } = "post_not_found";
        public Guid PostId { get; }

        public PostNotFoundException(Guid postId) : base($"Post with ID: '{postId}' was not found.")
        {
            PostId = postId;
        }
    }
}