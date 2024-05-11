
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Dto
{
    public class ReactionsSummaryDto(int nrReactions, ReactionType? dominant,
            Guid? authUserReactionId, ReactionType? authUserReactionType)
    {
        public int NumberOfReactions {get;set;} = nrReactions;
        public ReactionType? DominantReaction {get;set;} = dominant;
        public Guid? AuthUserReactionId {get;set;} = authUserReactionId;
        public ReactionType? AuthUserReactionType {get;set;} = authUserReactionType;
    }    
}
