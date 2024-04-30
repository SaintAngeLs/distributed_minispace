namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostTextContentException : DomainException
    {
        public override string Code { get; } = "invalid_post_text_content";
        public Guid Id { get; }
        
        public InvalidPostTextContentException(Guid id) : base(
            $"Post with id: {id} has invalid content. Its length should be between 1 and 5000 characters.")
        {
            Id = id;
        }
    }
}
