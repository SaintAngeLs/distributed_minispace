using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IOrganizationEventPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByOrganizationEventIdAsync(Guid organizationId, Guid eventId);
        Task<(IEnumerable<Post> posts, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseOrganizationEventPostsAsync(
            int pageNumber, 
            int pageSize, 
            Guid organizationId, 
            Guid eventId, 
            IEnumerable<string> sortBy, 
            string direction);
    }
}
