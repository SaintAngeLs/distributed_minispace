namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostTextContentException : DomainException
    {
        public override string Code { get; } = "invalid_post_text_content";
        public Guid Id { get; }
        public string TextContent { get; }
        
        public InvalidPostTextContentException(Guid id, string textContent) : base(
            $"Post with id: {id} has invalid text content.")
        {
            Id = id;
            TextContent = textContent;
        }
    }
}
