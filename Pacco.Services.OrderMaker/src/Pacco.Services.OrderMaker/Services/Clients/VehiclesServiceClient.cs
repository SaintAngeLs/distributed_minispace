using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Pacco.Services.OrderMaker.DTO;

namespace Pacco.Services.OrderMaker.Services.Clients
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

        public async Task<VehicleDto> GetBestAsync()
        {
            var vehicles = await _httpClient.GetAsync<PagedResult<VehicleDto>>($"{_url}/vehicles");
            var bestVehicle = vehicles?.Items?.FirstOrDefault(); // typical AI in a startup
            if (bestVehicle is null)
            {
                throw new InvalidOperationException("The best vehicle was not found.");
            }

            return bestVehicle;
        }
    }
}