namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class UnauthorizedPostOperationException : AppException
    {
        public override string Code { get; } = "not_allowed_post_operation";
        public Guid PostId { get; }
        public Guid UserId { get; }

        public UnauthorizedPostOperationException(Guid postId, Guid userId)
            : base($"Not allowed operation on post with id: '{postId}' by user with id: '{userId}'.")
        {
            PostId = postId;
            UserId = userId;
        }
    }
}