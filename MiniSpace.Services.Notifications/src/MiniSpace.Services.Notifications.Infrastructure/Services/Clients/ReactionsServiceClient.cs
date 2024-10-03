using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class ReactionsServiceClient : IReactionsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _reactionsServiceUrl;

        public ReactionsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _reactionsServiceUrl = options.Services["reactions"];
        }

        public async Task<IEnumerable<ReactionDto>> GetReactionsAsync()
        {
            var url = $"{_reactionsServiceUrl}/reactions";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<IEnumerable<ReactionDto>>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Reactions Service. Status Code: {response.StatusCode}");
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
                Console.WriteLine($"Error deserializing the response from Reactions Service: {ex.Message}\nJSON: {json}");
                return null;
            }
        }
    }
}
