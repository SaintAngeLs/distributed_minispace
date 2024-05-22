namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class CommentAlreadyAddedException : AppException
    {
        public override string Code { get; } = "comment_already_added";
        public Guid CommentId { get; }

        public CommentAlreadyAddedException(Guid commentId)
            : base($"Comment with id: {commentId} was already added.")
        {
            CommentId = commentId;
        }
    }  
}