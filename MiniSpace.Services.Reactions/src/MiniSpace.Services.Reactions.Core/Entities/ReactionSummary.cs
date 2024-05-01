

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class ReactionSummary
    {
        public ReactionSummary() 
        {
            dictionary = new Dictionary<ReactionType, int>();
        }

        private Dictionary<ReactionType, int> dictionary;

        public int GetNrReactions(ReactionType reactionType)
        {
            int nrReactions = 0;
            dictionary.TryGetValue(reactionType, out nrReactions);
            return nrReactions;
        }

        public void SetNrReactions(ReactionType reactionType, int nrReactions) {
            dictionary[reactionType] = nrReactions;
        }
    }
}