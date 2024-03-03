using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Application.Services.Clients;

namespace Pacco.Services.Orders.Infrastructure.Services.Clients
{
    public class ParcelsServiceClient : IParcelsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public ParcelsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["parcels"];
        }

        public Task<ParcelDto> GetAsync(Guid id)
            => _httpClient.GetAsync<ParcelDto>($"{_url}/parcels/{id}");
    }
}