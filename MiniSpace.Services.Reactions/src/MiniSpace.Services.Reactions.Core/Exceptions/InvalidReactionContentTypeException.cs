namespace MiniSpace.Services.Reactions.Core.Exceptions
{
    public class InvalidReactionContentTypeException : DomainException
    {
        public override string Code { get; } = "invalid_post_state";
        public string InvalidReactionContentType { get; }

        public InvalidReactionContentTypeException(string invalidReactionContentType) : base(
            $"String: {invalidReactionContentType} cannot be parsed to valid reaction content type.")
        {
            InvalidReactionContentType = invalidReactionContentType;
        }
    }    
}
