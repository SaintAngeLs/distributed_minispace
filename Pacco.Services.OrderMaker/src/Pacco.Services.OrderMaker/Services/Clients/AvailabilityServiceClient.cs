using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.OrderMaker.DTO;

namespace Pacco.Services.OrderMaker.Services.Clients
{
    public class AvailabilityServiceClient : IAvailabilityServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public AvailabilityServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["availability"];
        }
            
        public Task<ResourceDto> GetResourceReservationsAsync(Guid resourceId)
            => _httpClient.GetAsync<ResourceDto>($"{_url}/resources/{resourceId}");
    }
}