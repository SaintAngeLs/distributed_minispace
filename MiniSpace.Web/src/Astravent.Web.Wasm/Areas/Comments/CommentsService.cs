using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Astravent.Web.Wasm.Areas.Comments.CommandDto;
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

        public Task<PagedResponseDto<CommentDto>> SearchRootCommentsAsync(SearchRootCommentsCommand command)
        {
            var queryString = ToQueryString(command);

            // Log the query string to the console
            Console.WriteLine($"Sending request with query string: comments/search{queryString}");

            return _httpClient.GetAsync<PagedResponseDto<CommentDto>>($"comments/search{queryString}");
        }


        private string ToQueryString(SearchRootCommentsCommand command)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["ContextId"] = command.ContextId.ToString();
            query["CommentContext"] = command.CommentContext;

            // Flatten the PageableDto into individual query parameters
            if (command.Pageable != null)
            {
                query["Page"] = command.Pageable.Page.ToString();
                query["Size"] = command.Pageable.Size.ToString();
                
                if (command.Pageable.Sort != null)
                {
                    // Pass SortBy as a comma-separated list
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

        public Task<HttpResponse<object>> AddLikeAsync(AddLikeDto command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"comments/{command.CommentId}/like", command);
        }

        public Task DeleteLikeAsync(Guid commentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"comments/{commentId}/like");
        }
    }    
}
