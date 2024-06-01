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
        Task<bool> ExistsAsync(Guid id);
        Task<(IEnumerable<Post> posts, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseCommentsAsync(int pageNumber, int pageSize, 
            IEnumerable<Guid> eventsIds, IEnumerable<string> sortBy, string direction);
    }    
}
