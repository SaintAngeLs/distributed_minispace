using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class ReportsServiceClient : IReportsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _reportsServiceUrl;

        public ReportsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _reportsServiceUrl = options.Services["reports"];
        }

        public async Task<ReportDto> GetReportAsync(Guid studentId)
        {
            var url = $"{_reportsServiceUrl}/reports/students/{studentId}";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<ReportDto>(response);
        }

        public async Task<IEnumerable<ReportDto>> GetReportsByUserIdAsync(Guid userId)
        {
            var url = $"{_reportsServiceUrl}/reports/students/{userId}";
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<IEnumerable<ReportDto>>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Reports Service. Status Code: {response.StatusCode}");
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
                Console.WriteLine($"Error deserializing the response from Reports Service: {ex.Message}\nJSON: {json}");
                return null;
            }
        }
    }
}
