using System.Formats.Tar;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    // content not specified
    public interface IReactionRepository
    {
        Task<ReactionSummary> GetReactionSummaryAsync(Guid id);
        Task<ReactionData> GetReactions(Guid id);
        
    }    
}
