namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostMediaContentException : DomainException
    {
        public override string Code { get; } = "invalid_post_media_content";
        public Guid Id { get; }
        public string MediaContent { get; }
        
        public InvalidPostMediaContentException(Guid id, string mediaContent) : base(
            $"Post with id: {id} has invalid media content.")
        {
            Id = id;
            MediaContent = mediaContent;
        }
    }
}
