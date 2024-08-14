using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IOrganizationEventPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByOrganizationEventIdAsync(Guid organizationId, Guid eventId);
        Task<PagedResponse<Post>> BrowseOrganizationEventPostsAsync(BrowseRequest request);
    }
}
