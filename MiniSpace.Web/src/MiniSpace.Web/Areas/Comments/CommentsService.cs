using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Comments.CommandsDto;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
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

        public Task<HttpResponse<PagedResponseDto<CommentDto>>> SearchRootCommentsAsync(SearchRootCommentsCommand command)
        {
            return _httpClient.PostAsync<SearchRootCommentsCommand, PagedResponseDto<CommentDto>>("comments/search", command);
        }

        public Task<HttpResponse<PagedResponseDto<CommentDto>>> SearchSubCommentsAsync(SearchSubCommentsCommand command)
        {
            return _httpClient.PostAsync<SearchSubCommentsCommand, PagedResponseDto<CommentDto>>("comments/search", command);
        }
        
        public Task<CommentDto> GetCommentAsync(Guid commentId)
        {
            return _httpClient.GetAsync<CommentDto>($"comments/{commentId}");
        }
        
        public Task<HttpResponse<object>> CreateCommentAsync(CreateCommentCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("comments", command);
        }

        public Task<HttpResponse<object>> UpdateCommentAsync(UpdateCommentCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"comments/{command.CommentId}", command);
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
