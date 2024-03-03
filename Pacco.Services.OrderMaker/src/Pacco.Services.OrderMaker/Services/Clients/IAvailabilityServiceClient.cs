using System;
using System.Threading.Tasks;
using Pacco.Services.OrderMaker.DTO;

namespace Pacco.Services.OrderMaker.Services.Clients
{
    public interface IAvailabilityServiceClient
    {
        Task<ResourceDto> GetResourceReservationsAsync(Guid resourceId);
    }
}