namespace MiniSpace.Services.Reactions.Core.Exceptions
{
    public class InvalidReactionTypeException : DomainException
    {
        public override string Code { get; } = "invalid_post_state";
        public string InvalidReactionType { get; }

        public InvalidReactionTypeException(string invalidReactionType) : base(
            $"String: {invalidReactionType} cannot be parsed to valid reaction type.")
        {
            InvalidReactionType = invalidReactionType;
        }
    }    
}
