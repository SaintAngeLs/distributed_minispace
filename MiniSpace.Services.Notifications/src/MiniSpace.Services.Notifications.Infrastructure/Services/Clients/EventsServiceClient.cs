using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class EventsServiceClient : IEventsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _eventsServiceUrl;

        public EventsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _eventsServiceUrl = options.Services["events"];
        }

        public async Task<IEnumerable<StudentDto>> GetParticipantsAsync(Guid eventId)
        {
            var url = $"{_eventsServiceUrl}/events/{eventId}/participants";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<IEnumerable<StudentDto>>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Events Service. Status Code: {response.StatusCode}");
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
                Console.WriteLine($"Error deserializing the response from Events Service: {ex.Message}");
                return null;
            }
        }
    }
}
