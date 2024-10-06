using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Enums;

namespace Astravent.Web.Wasm.DTO
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
