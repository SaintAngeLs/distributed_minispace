﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.Areas.Identity;
using MiniSpacePwa.Data.Events;
using MiniSpacePwa.Data.Posts;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Wrappers;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Posts
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
            IEnumerable<Guid> mediaFiles, string state, DateTime? publishDate)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("posts", new {postId, eventId, organizerId, textContent,
                mediaFiles, state, publishDate});
        }
        
        public Task<HttpResponse<PagedResponseDto<IEnumerable<PostDto>>>> SearchPostsAsync(SearchPosts searchPosts)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchPosts, PagedResponseDto<IEnumerable<PostDto>>>("posts/search", searchPosts);
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
