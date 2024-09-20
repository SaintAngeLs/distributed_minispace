using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IUserReactionsHistoryRepository
    {
        Task SaveReactionAsync(Guid userId, Reaction reaction);

        Task<IEnumerable<Reaction>> GetUserReactionsAsync(Guid userId);

        Task<PagedResponse<Reaction>> GetUserReactionsPagedAsync(Guid userId, int pageNumber, int pageSize);
        
        Task DeleteReactionAsync(Guid userId, Guid reactionId);
    }
}
