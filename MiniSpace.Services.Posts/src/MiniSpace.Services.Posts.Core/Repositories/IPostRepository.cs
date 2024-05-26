using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetAsync(Guid id);
        Task<IEnumerable<Post>> GetToUpdateAsync();
        Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Guid id);
    }    
}
