namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class InvalidPostIdException : AppException
    {
        public Guid PostId { get; }

        public InvalidPostIdException(Guid postId) : base($"Invalid post id: {postId}")
        {
            PostId = postId;
        }
    }
}