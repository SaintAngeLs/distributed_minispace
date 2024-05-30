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

        public async Task<EventParticipantsDto> GetParticipantsAsync(Guid eventId)
        {
            var url = $"{_eventsServiceUrl}/events/{eventId}/participants";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<EventParticipantsDto>(response);
        }

        public async Task<EventDto> GetEventAsync(Guid eventId)
        {
            var url = $"{_eventsServiceUrl}/events/{eventId}";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<EventDto>(response);  
        }
  
        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                // Console.WriteLine($"Error fetching data from Events Service. Status Code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            // Console.WriteLine("JSON Response: " + json); 

            try
            {
                var responseObject = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseObject == null)
                {
                    // Console.WriteLine("Deserialized object is null. Possibly empty JSON.");
                    return null;
                }

                var jsonString = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
                // Console.WriteLine("Deserialized JSON Object: " + jsonString);

                return responseObject;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing the response from Events Service: {ex.Message}\nJSON: {json}");
                return null;
            }
        }
    }
}