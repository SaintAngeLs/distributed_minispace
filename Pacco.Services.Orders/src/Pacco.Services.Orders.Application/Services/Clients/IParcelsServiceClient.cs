using System;
using System.Threading.Tasks;
using Pacco.Services.Orders.Application.DTO;

namespace Pacco.Services.Orders.Application.Services.Clients
{
    public interface IParcelsServiceClient
    {
        Task<ParcelDto> GetAsync(Guid id);
    }
}