namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class UnauthorizedPostAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_post_access";
        public Guid PostId { get; }
        public Guid UserId { get; }

        public UnauthorizedPostAccessException(Guid postId, Guid userId)
            : base($"Unauthorized access to post with id: '{postId}' by user with id: '{userId}'.")
        {
            PostId = postId;
            UserId = userId;
        }
    }
}
