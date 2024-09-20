
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ReactionsSummaryDto(int numerOfReactions, ReactionType? dominant, Guid? authUserReactionId, ReactionType? authUserReactionType)
    {
        public int NumberOfReactions { get; set; } = numerOfReactions;
        public ReactionType? DominantReaction { get; set; } = dominant;
        public Guid? AuthUserReactionId { get; set; } = authUserReactionId;
        public ReactionType? AuthUserReactionType { get; set; } = authUserReactionType;
        public Dictionary<ReactionType, int> ReactionsWithCounts { get; set; } = new Dictionary<ReactionType, int>();    
    }    
}
