using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Post post);
        Task DeleteAsync(Guid id);
    }
}