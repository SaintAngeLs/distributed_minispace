using System;
using MiniSpacePwa.DTO.Enums;

namespace MiniSpacePwa.DTO
{
    public class ReactionsSummaryDto
    {
        public int NumberOfReactions { get; set; }
        public ReactionType? DominantReaction { get; set; }
        public Guid? AuthUserReactionId { get; set; }
        public ReactionType? AuthUserReactionType { get; set; }
    }    
}
