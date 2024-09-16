using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Notifications.Application.Dto.Comments;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class CommentsServiceClient : ICommentsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _commentsServiceUrl;

        public CommentsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _commentsServiceUrl = options.Services["comments"];
        }

        public async Task<CommentDto> GetCommentAsync(Guid commentId)
        {
            var url = $"{_commentsServiceUrl}/comments/{commentId}";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<CommentDto>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Comments Service. Status Code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("JSON Response: " + json);

            try
            {
                var responseObject = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseObject == null)
                {
                    Console.WriteLine("Deserialized object is null. Possibly empty JSON.");
                    return null;
                }

                var jsonString = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("Deserialized JSON Object: " + jsonString);

                return responseObject;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing the response from Comments Service: {ex.Message}\nJSON: {json}");
                return null;
            }
        }
    }
}
