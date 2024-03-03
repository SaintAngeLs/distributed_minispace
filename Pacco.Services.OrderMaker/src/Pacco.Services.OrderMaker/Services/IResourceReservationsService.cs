using System;
using System.Threading.Tasks;
using Pacco.Services.OrderMaker.DTO;

namespace Pacco.Services.OrderMaker.Services
{
    public interface IResourceReservationsService
    {
        Task<ReservationDto> GetBestAsync(Guid resourceId);
    }
}