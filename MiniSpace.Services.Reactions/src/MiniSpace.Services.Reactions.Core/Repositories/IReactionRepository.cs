using System.Formats.Tar;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionRepository
    {
        Task<(int NumberOfReactions, ReactionType DominantReaction)> GetReactionSummaryAsync(Guid contentId, ReactionContentType contentType);
        Task<IEnumerable<Reaction>> GetReactionsAsync(Guid contentId, ReactionContentType contentType);
        Task<Reaction> GetAsync(Guid id);
        Task AddAsync(Reaction reaction);
        Task DeleteAsync(Guid id);
        
    }    
}