using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class ReactionNotFoundException : AppException
    {
        public override string Code { get; } = "reaction_not_found";
        public Guid ReactionId;

        public ReactionNotFoundException(Guid reactionId) : 
                    base($"Reaction with id: {reactionId} was not found.")
        {
            ReactionId = reactionId;
        }
    }    
}
