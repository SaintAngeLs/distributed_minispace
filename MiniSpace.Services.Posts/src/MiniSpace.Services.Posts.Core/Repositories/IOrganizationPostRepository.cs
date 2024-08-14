using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IOrganizationPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByOrganizationIdAsync(Guid organizationId);
        Task<(IEnumerable<Post> posts, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseOrganizationPostsAsync(
            int pageNumber, 
            int pageSize, 
            Guid organizationId, 
            IEnumerable<string> sortBy, 
            string direction);
    }
}
