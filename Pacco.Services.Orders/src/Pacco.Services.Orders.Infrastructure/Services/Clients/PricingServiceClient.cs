using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Application.Services.Clients;

namespace Pacco.Services.Orders.Infrastructure.Services.Clients
{
    public class PricingServiceClient : IPricingServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public PricingServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["pricing"];
        }

        public Task<OrderPricingDto> GetOrderPriceAsync(Guid customerId, decimal orderPrice)
            => _httpClient.GetAsync<OrderPricingDto>($"{_url}/pricing?customerId={customerId}&orderPrice={orderPrice}");
    }
}