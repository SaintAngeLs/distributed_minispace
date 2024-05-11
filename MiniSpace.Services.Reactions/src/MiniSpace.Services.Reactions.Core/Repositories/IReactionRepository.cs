using System.Formats.Tar;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionRepository
    {
        Task<IEnumerable<Reaction>> GetReactionsAsync(Guid contentId, ReactionContentType contentType);
        Task<Reaction> GetAsync(Guid id);
        Task AddAsync(Reaction reaction);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid contentId, ReactionContentType contentType, Guid studentId);
        
    }    
}
