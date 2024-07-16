using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.Areas.Identity;
using MiniSpacePwa.DTO;
using MiniSpacePwa.Data.Comments;
using MiniSpacePwa.DTO.Wrappers;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Comments
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

        public Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchRootCommentsAsync(Guid contextId,
            string commentContext, PageableDto pageable)
        {
            return _httpClient.PostAsync<SearchRootComments, PagedResponseDto<IEnumerable<CommentDto>>>("comments/search", 
                new (contextId, commentContext, pageable));
        }

        public Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchSubCommentsAsync(Guid contextId,
            string commentContext, Guid parentId, PageableDto pageable)
        {
            return _httpClient.PostAsync<SearchSubComments, PagedResponseDto<IEnumerable<CommentDto>>>("comments/search", 
                new (contextId, commentContext, parentId, pageable));
        }
        
        public Task<CommentDto> GetCommentAsync(Guid commentId)
        {
            return _httpClient.GetAsync<CommentDto>($"comments/{commentId}");
        }
        
        public Task<HttpResponse<object>> CreateCommentAsync(Guid commentId, Guid contextId, string commentContext,
            Guid studentId, Guid parentId, string comment)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("comments",
                new { commentId, contextId, commentContext, studentId, parentId, comment });
        }

        public Task<HttpResponse<object>> UpdateCommentAsync(Guid commentId, string textContent)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"comments/{commentId}", new { commentId, textContent});
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
