using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionsOrganizationsPostRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Post post);
    }    
}
