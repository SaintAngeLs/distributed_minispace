using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId);
        Task<(IEnumerable<Post> posts, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseUserPostsAsync(
            int pageNumber, 
            int pageSize, 
            Guid userId, 
            IEnumerable<string> sortBy, 
            string direction);
    }
}
