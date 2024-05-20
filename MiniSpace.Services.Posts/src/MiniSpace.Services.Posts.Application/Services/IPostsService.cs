using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Application.Services
{
    public interface IPostsService
    {
        Task<PagedResponse<IEnumerable<PostDto>>> BrowsePostsAsync(SearchPosts command);
    }
}