using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Dto.Posts;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class PostsServiceClient : IPostsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _postsServiceUrl;

        public PostsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _postsServiceUrl = options.Services["posts"];
        }

        public async Task<PostDto> GetPostAsync(Guid postId)
        {
            var url = $"{_postsServiceUrl}/posts/{postId}";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<PostDto>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                // Console.WriteLine($"Error fetching data from Posts Service. Status Code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                // Console.WriteLine($"Error deserializing the response from Posts Service: {ex.Message}");
                return null;
            }
        }
    }
}
