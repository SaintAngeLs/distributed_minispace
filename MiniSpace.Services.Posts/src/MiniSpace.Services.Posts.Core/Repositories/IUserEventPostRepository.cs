using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserEventPostRepository : IPostRepository
    {
        Task<IEnumerable<Post>> GetByUserEventIdAsync(Guid userId, Guid eventId);
        Task<PagedResponse<Post>> BrowseUserEventPostsAsync(BrowseRequest request);
    }
}
