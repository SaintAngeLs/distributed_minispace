using System;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionsUserPostRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task<Post> GetByIdAsync(Guid id); 
        Task AddAsync(Post post);
        Task UpdateAsync(Post post); 
        Task DeleteAsync(Guid id); 
    }
}
