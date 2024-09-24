using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Application.Services
{
    public interface IPostsService
    {
        Task<PagedResponse<PostDto>> BrowsePostsAsync(BrowseRequest request);
        Task<Post> CreatePostAsync(CreatePost command);
        Task<Post> UpdatePostAsync(UpdatePost command);
        Task<Post> RepostPostAsync(RepostCommand command);
        Task DeletePostAsync(DeletePost command);
    }
}
