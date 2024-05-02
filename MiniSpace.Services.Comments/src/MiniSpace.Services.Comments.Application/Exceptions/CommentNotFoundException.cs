namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class CommentNotFoundException : AppException
    {
        public override string Code { get; } = "comment_not_found";
        public Guid Id { get; }

        public CommentNotFoundException(Guid id) : base($"Comment with id: {id} was not found.")
        {
            Id = id;
        }
    }    
}
