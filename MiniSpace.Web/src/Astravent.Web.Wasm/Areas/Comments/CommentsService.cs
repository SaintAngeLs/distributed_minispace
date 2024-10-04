using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Astravent.Web.Wasm.Areas.Comments.CommandsDto;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.DTO.Comments;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Comments
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

        public async Task<PagedResponseDto<CommentDto>> SearchRootCommentsAsync(SearchRootCommentsCommand command)
        {
            var queryString = ToQueryString(command);

            Console.WriteLine($"Sending request with query string: comments/search{queryString}");

            return await _httpClient.GetAsync<PagedResponseDto<CommentDto>>($"comments/search{queryString}");
        }


        private string ToQueryString(SearchRootCommentsCommand command)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["ContextId"] = command.ContextId.ToString();
            query["CommentContext"] = command.CommentContext;

            if (command.Pageable != null)
            {
                query["Page"] = command.Pageable.Page.ToString();
                query["Size"] = command.Pageable.Size.ToString();
                
                if (command.Pageable.Sort != null)
                {
                    if (command.Pageable.Sort.SortBy != null && command.Pageable.Sort.SortBy.Any())
                    {
                        query["SortBy"] = string.Join(",", command.Pageable.Sort.SortBy);
                    }
                    query["Direction"] = command.Pageable.Sort.Direction;
                }
            }

            return "?" + query.ToString();
        }


        public Task<HttpResponse<PagedResponseDto<CommentDto>>> SearchSubCommentsAsync(SearchSubCommentsCommand command)
        {
            return _httpClient.PostAsync<SearchSubCommentsCommand, PagedResponseDto<CommentDto>>("comments/search", command);
        }
        
        public async Task<CommentDto> GetCommentAsync(Guid commentId)
        {
            return await _httpClient.GetAsync<CommentDto>($"comments/{commentId}");
        }
        
        public async Task<HttpResponse<object>> CreateCommentAsync(CreateCommentCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>("comments", command);
        }

        public async Task<HttpResponse<object>> UpdateCommentAsync(UpdateCommentCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<object, object>($"comments/{command.CommentId}", command);
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"comments/{commentId}");
        }

        public async Task<HttpResponse<object>> AddLikeAsync(AddLikeDto command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>($"comments/{command.CommentId}/like", command);
        }

        public async Task DeleteLikeAsync(Guid commentId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"comments/{commentId}/like");
        }
    }    
}
