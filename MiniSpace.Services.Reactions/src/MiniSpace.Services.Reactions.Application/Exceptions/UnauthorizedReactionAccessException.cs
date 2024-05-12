namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class UnauthorizedReactionAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_reaction_access";
        public Guid UserId { get; }
        public Guid ReactionId {get;}

        public UnauthorizedReactionAccessException(Guid reactionId, Guid userId) : 
            base($"Unauthorized access to reaction with id: '{reactionId}' by user with id: '{userId}'.")
        {
            ReactionId = reactionId;
            UserId = userId;
        }
    }    
}
