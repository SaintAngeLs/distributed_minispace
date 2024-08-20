using System;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionsOrganizationsPostCommentsRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task<Reaction> GetByIdAsync(Guid id);
        Task AddAsync(Reaction reaction);
        Task UpdateAsync(Reaction reaction);
        Task DeleteAsync(Guid id);
    }
}
