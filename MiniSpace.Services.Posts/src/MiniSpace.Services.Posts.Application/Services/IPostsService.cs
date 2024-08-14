using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Application.Services
{
    public interface IPostsService
    {
        /// <summary>
        /// Browses posts based on the given request parameters.
        /// </summary>
        /// <param name="request">The browsing request containing filtering, sorting, and pagination information.</param>
        /// <returns>A paged response containing the posts matching the criteria.</returns>
        Task<PagedResponse<PostDto>> BrowsePostsAsync(BrowseRequest request);
    }
}
