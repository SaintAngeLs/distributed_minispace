using System;
using System.Threading.Tasks;
using Pacco.Services.Pricing.Api.DTO;

namespace Pacco.Services.Pricing.Api.Services.Clients
{
    public interface ICustomersServiceClient
    {
        Task<CustomerDto> GetAsync(Guid id);
    }
}