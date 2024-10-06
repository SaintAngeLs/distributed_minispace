using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Posts.CommandsDto;
using Astravent.Web.Wasm.Data.Events;
using Astravent.Web.Wasm.Data.Posts;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Enums.Posts;
using Astravent.Web.Wasm.DTO.Posts;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Posts
{
    public class PostsService: IPostsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public PostsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public Task<PostDto> GetPostAsync(Guid postId)
        {
            return _httpClient.GetAsync<PostDto>($"posts/{postId}");
        }
        
        public Task ChangePostStateAsync(Guid postId, string state, DateTime publishDate)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync($"posts/{postId}/state/{state}", new {postId, state, publishDate});
        }
        
        public Task<HttpResponse<object>> CreatePostAsync(CreatePostCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<CreatePostCommand, object>("posts", command);
        }

        public Task<HttpResponse<object>> RepostPostAsync(RepostCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<RepostCommand, object>("posts/repost", command);
        }
        
        public async Task<HttpResponse<PagedResponseDto<PostDto>>> SearchPostsAsync(SearchPosts searchPosts)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

            var query = HttpUtility.ParseQueryString(string.Empty);
            if (searchPosts.UserId.HasValue)
                query["UserId"] = searchPosts.UserId.ToString();
            if (searchPosts.OrganizationId.HasValue)
                query["OrganizationId"] = searchPosts.OrganizationId.ToString();
            if (searchPosts.EventId.HasValue)
                query["EventId"] = searchPosts.EventId.ToString();

            query["PageNumber"] = searchPosts.Pageable.Page.ToString();
            query["PageSize"] = searchPosts.Pageable.Size.ToString();
            if (searchPosts.Pageable.Sort?.SortBy != null)
                query["SortBy"] = string.Join(",", searchPosts.Pageable.Sort.SortBy);
            query["Direction"] = searchPosts.Pageable.Sort?.Direction;

            string queryString = query.ToString();
            string url = $"posts/search?{queryString}";

            try
            {
                var result = await _httpClient.GetAsync<PagedResponseDto<PostDto>>(url);
                return new HttpResponse<PagedResponseDto<PostDto>>(result);
            }
            catch (Exception ex)
            {
                return new HttpResponse<PagedResponseDto<PostDto>>(new ErrorMessage
                {
                    Code = ex.Message,
                    Reason = ex.Message
                });
            }
        }

        public async Task<HttpResponse<PagedResponseDto<PostDto>>> GetUserFeedAsync(Guid userId, int pageNumber, 
            int pageSize, string sortBy = "PublishDate", string direction = "asc")
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            
            _httpClient.SetAccessToken(accessToken);
            
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["PageNumber"] = pageNumber.ToString();
            query["PageSize"] = pageSize.ToString();
            query["SortBy"] = sortBy;
            query["Direction"] = direction;

            string queryString = query.ToString();
            string url = $"posts/users/{userId}/feed?{queryString}";

            try
            {
                var result = await _httpClient.GetAsync<PagedResponseDto<PostDto>>(url);
                return new HttpResponse<PagedResponseDto<PostDto>>(result);
            }
            catch (Exception ex)
            {
                return new HttpResponse<PagedResponseDto<PostDto>>(new ErrorMessage
                {
                    Code = ex.Message,
                    Reason = ex.Message
                });
            }
        }

        public async Task<HttpResponse<PagedResponseDto<PostDto>>> GetCurrentUserFeedAsync(int pageNumber, 
            int pageSize, string sortBy = "PublishDate", string direction = "asc")
        {
            Guid userId;

            try
            {
                userId = await _identityService.GetCurrentUserIdFromJwtAsync();
                if (userId == Guid.Empty)
                {
                    throw new InvalidOperationException("User ID could not be retrieved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get current user ID: {ex.Message}");
                return new HttpResponse<PagedResponseDto<PostDto>>(new ErrorMessage
                {
                    Code = ex.Message,
                    Reason = "User not authenticated or ID could not be retrieved."
                });
            }

            var accessToken = await _identityService.GetAccessTokenAsync();
            
            _httpClient.SetAccessToken(accessToken);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["PageNumber"] = pageNumber.ToString();
            query["PageSize"] = pageSize.ToString();
            query["SortBy"] = sortBy;
            query["Direction"] = direction;

            string queryString = query.ToString();
            string url = $"posts/users/{userId}/feed?{queryString}";

            try
            {
                var result = await _httpClient.GetAsync<PagedResponseDto<PostDto>>(url);
                return new HttpResponse<PagedResponseDto<PostDto>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user feed: {ex.Message}");
                return new HttpResponse<PagedResponseDto<PostDto>>(new ErrorMessage
                {
                    Code = ex.Message,
                    Reason = ex.Message
                });
            }
        }




        public Task DeletePostAsync(Guid postId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"posts/{postId}");
        }

        public Task<IEnumerable<PostDto>> GetPostsAsync(Guid eventId)
        {
            return _httpClient.GetAsync<IEnumerable<PostDto>>($"posts?eventId={eventId}");
        }

        public Task<HttpResponse<object>> UpdatePostAsync(UpdatePostCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<UpdatePostCommand, object>($"posts/{command.PostId}", command);
        }
    }
}