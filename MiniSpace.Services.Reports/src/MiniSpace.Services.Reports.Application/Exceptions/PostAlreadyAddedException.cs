namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class PostAlreadyAddedException : AppException
    {
        public override string Code { get; } = "post_already_added";
        public Guid PostId { get; }
    
        public PostAlreadyAddedException(Guid postId)
            : base($"Post with id: {postId} was already added.")
        {
            PostId = postId;
        }
    }  
}