using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.DTO
{
    public class ReactionsSummaryDto
    {
        public int NumberOfReactions { get; set; }
        public ReactionType? DominantReaction { get; set; }
        public Guid? AuthUserReactionId { get; set; }
        public ReactionType? AuthUserReactionType { get; set; }
        public Dictionary<ReactionType, int> ReactionsWithCounts { get; set; } = new Dictionary<ReactionType, int>();   
    }    
}
