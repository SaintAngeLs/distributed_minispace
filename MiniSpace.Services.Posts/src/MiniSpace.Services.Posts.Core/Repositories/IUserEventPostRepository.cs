using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserEventPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByUserEventIdAsync(Guid userId, Guid eventId);
        Task<(IEnumerable<Post> posts, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseUserEventPostsAsync(
            int pageNumber, 
            int pageSize, 
            Guid userId, 
            Guid eventId, 
            IEnumerable<string> sortBy, 
            string direction);
    }
}
