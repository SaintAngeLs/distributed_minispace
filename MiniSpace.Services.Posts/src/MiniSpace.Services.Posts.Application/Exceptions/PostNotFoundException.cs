namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class PostNotFoundException : AppException
    {
        public override string Code { get; } = "post_not_found";
        public Guid Id { get; }

        public PostNotFoundException(Guid id) : base($"Post with id: {id} was not found.")
        {
            Id = id;
        }
    }    
}
