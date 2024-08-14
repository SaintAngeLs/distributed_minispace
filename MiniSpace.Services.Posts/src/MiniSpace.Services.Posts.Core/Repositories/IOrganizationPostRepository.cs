using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IOrganizationPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByOrganizationIdAsync(Guid organizationId);
        Task<PagedResponse<Post>> BrowseOrganizationPostsAsync(BrowseRequest request);
    }
}
