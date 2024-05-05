
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Dto
{
    public class ReactionsSummaryDto(int nrReactions, ReactionType dominant)
    {
        public int NumberOfReactions {get;set;} = nrReactions;
        public ReactionType DominantReaction {get;set;} = dominant;
    }    
}
