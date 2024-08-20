using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Posts.CommandsDto;
using MiniSpace.Web.Data.Posts;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Posts
{
    public interface IPostsService
    {
        Task<PostDto> GetPostAsync(Guid postId);
        Task ChangePostStateAsync(Guid postId, string state, DateTime publishDate); 
        Task<HttpResponse<object>> CreatePostAsync(CreatePostCommand command);
        Task<HttpResponse<PagedResponseDto<PostDto>>> SearchPostsAsync(SearchPosts searchPosts);
        Task DeletePostAsync(Guid postId);
        Task<IEnumerable<PostDto>> GetPostsAsync(Guid eventId);
        Task<HttpResponse<object>> UpdatePostAsync(Guid postId, string textContent, IEnumerable<Guid> mediaFiles);
    }
}