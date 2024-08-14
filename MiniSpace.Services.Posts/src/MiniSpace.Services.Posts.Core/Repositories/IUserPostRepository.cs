using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId);
        Task<PagedResponse<Post>> BrowseUserPostsAsync(BrowseRequest request);
    }
}
