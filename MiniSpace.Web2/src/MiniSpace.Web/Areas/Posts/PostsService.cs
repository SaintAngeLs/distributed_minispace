using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.Areas.Posts.CommandsDto;
using MiniSpace.Web.Data.Events;
using MiniSpace.Web.Data.Posts;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Enums.Posts;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Posts
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
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

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


        public Task DeletePostAsync(Guid postId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"posts/{postId}");
        }

        public Task<IEnumerable<PostDto>> GetPostsAsync(Guid eventId)
        {
            return _httpClient.GetAsync<IEnumerable<PostDto>>($"posts?eventId={eventId}");
        }

        public Task<HttpResponse<object>> UpdatePostAsync(Guid postId, string textContent, IEnumerable<Guid> mediaFiles)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"posts/{postId}", new {postId, textContent, mediaFiles});
        }
    }
}