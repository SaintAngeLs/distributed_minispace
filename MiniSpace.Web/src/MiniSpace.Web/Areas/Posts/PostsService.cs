using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
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
        
        public Task<HttpResponse<object>> CreatePostAsync(Guid postId, Guid eventId, Guid organizerId, string textContent,
            string mediaContext, string state, DateTime? publishDate)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("posts", new {postId, eventId, organizerId, textContent,
                mediaContext, state, publishDate});
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

        public Task<HttpResponse<object>> UpdatePostAsync(Guid postId, string textContent, string mediaContent)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"posts/{postId}", new {postId, textContent, mediaContent});
        }
    }
}