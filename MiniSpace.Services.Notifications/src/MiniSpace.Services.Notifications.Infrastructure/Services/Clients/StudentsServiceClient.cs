using System;
using System.Text.Json;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class StudentsServiceClient : IStudentsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;
        
        public StudentsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["students"];
        }
        
        public Task<UserDto> GetAsync(Guid id)
            => _httpClient.GetAsync<UserDto>($"{_url}/students/{id}");

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_url}/students");
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

    
            var jsonDocument = JsonDocument.Parse(json);
            if (jsonDocument.RootElement.TryGetProperty("results", out JsonElement resultsElement))
            {
                var students = JsonSerializer.Deserialize<IEnumerable<UserDto>>(resultsElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return students;
            }
            return null;
        }
    }
}