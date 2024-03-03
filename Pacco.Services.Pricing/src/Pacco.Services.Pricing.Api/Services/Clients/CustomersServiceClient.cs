using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Pricing.Api.DTO;

namespace Pacco.Services.Pricing.Api.Services.Clients
{
    internal sealed class CustomersServiceClient : ICustomersServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public CustomersServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["customers"];
        }

        public Task<CustomerDto> GetAsync(Guid id)
            => _httpClient.GetAsync<CustomerDto>($"{_url}/customers/{id}");
    }
}