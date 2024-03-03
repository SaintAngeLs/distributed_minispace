using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Application.Services.Clients;

namespace Pacco.Services.Orders.Infrastructure.Services.Clients
{
    public class VehiclesServiceClient : IVehiclesServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public VehiclesServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["vehicles"];
        }

        public Task<VehicleDto> GetAsync(Guid id)
            => _httpClient.GetAsync<VehicleDto>($"{_url}/vehicles/{id}");
    }
}