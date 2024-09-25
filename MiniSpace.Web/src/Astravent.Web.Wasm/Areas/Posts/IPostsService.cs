using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Posts.CommandsDto;
using Astravent.Web.Wasm.Data.Posts;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Posts
{
    public interface IPostsService
    {
        Task<PostDto> GetPostAsync(Guid postId);
        Task ChangePostStateAsync(Guid postId, string state, DateTime publishDate); 
        Task<HttpResponse<object>> CreatePostAsync(CreatePostCommand command);
        Task<HttpResponse<object>> RepostPostAsync(RepostCommand command);
        Task<HttpResponse<PagedResponseDto<PostDto>>> SearchPostsAsync(SearchPosts searchPosts);
        Task<HttpResponse<PagedResponseDto<PostDto>>> GetUserFeedAsync(Guid userId, int pageNumber, 
                int pageSize, string sortBy = "PublishDate", string direction = "asc");
        Task DeletePostAsync(Guid postId);
        Task<IEnumerable<PostDto>> GetPostsAsync(Guid eventId);
        Task<HttpResponse<object>> UpdatePostAsync(UpdatePostCommand command);
    }
}