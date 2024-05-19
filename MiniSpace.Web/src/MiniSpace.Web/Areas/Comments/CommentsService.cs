using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.Data.Comments;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public CommentsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchCommentsAsync(Guid contextId,
            string commentContext, Guid parentId, PageableDto pageable)
        {
            return _httpClient.PostAsync<SearchComments, PagedResponseDto<IEnumerable<EventDto>>>("comments/search", 
                new (contextId, commentContext, parentId, pageable));
        }

        public Task<HttpResponse<object>> CreateCommentAsync(Guid commentId, Guid contextId, string commentContext,
            Guid studentId, Guid parentId, string comment)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("comments",
                new { commentId, contextId, commentContext, studentId, parentId, comment });
        }

        public Task<HttpResponse<object>> UpdateCommentAsync(Guid commentId, string textContext)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"comments/{commentId}", new { commentId, textContext});
        }

        public Task DeleteCommentAsync(Guid commentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"comments/{commentId}");
        }

        public Task AddLikeAsync(Guid commentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"comments/{commentId}/like", new { commentId });
        }

        public Task DeleteLikeAsync(Guid commentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"comments/{commentId}/like");
        }
    }    
}
