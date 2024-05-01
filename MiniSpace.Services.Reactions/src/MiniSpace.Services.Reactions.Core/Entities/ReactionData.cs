

using System.Reflection.Metadata.Ecma335;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class ReactionData
    {
        public int NumberOfReactions {get; set;}
        public ReactionType DominantReaction {get; set;}
    }
}