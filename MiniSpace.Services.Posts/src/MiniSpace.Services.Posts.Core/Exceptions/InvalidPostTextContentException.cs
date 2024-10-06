namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostTextContentException : DomainException
    {
        public override string Code { get; } = "invalid_post_text_content";
        public Guid Id { get; }
        public int MaxTextLength { get; }

        public InvalidPostTextContentException(Guid id, int maxTextLength) 
            : base($"Post with id: '{id}' has invalid content. The length should be between 1 and {maxTextLength} characters.")
        {
            Id = id;
            MaxTextLength = maxTextLength;
        }
    }
}
