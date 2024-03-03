using System.Threading.Tasks;
using Pacco.Services.OrderMaker.DTO;

namespace Pacco.Services.OrderMaker.Services.Clients
{
    public interface IVehiclesServiceClient
    {
        Task<VehicleDto> GetBestAsync();
    }
}