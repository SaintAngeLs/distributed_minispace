namespace MiniSpace.Services.Reactions.Core.Exceptions
{
    public class InvalidReactionTargetTypeException : DomainException
    {
        public override string Code { get; } = "invalid_reaction_target_type";
        public string InvalidReactionTargetType { get; }

        public InvalidReactionTargetTypeException(string invalidReactionTargetType) 
            : base($"String: {invalidReactionTargetType} cannot be parsed to a valid reaction target type.")
        {
            InvalidReactionTargetType = invalidReactionTargetType;
        }
    }
}
