namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostContentException : DomainException
    {
        public override string Code { get; } = "invalid_post_content";
        public Guid Id { get; }
        
        public InvalidPostContentException(Guid id) : base(
            $"Post with id: {id} has invalid content.")
        {
            Id = id;
        }
    }
}
