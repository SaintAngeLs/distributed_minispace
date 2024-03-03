using Convey.CQRS.Queries;
using Pacco.Services.Vehicles.Application.DTO;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Application.Queries
{
    public class SearchVehicles : PagedQueryBase, IQuery<PagedResult<VehicleDto>>
    {
        public double PayloadCapacity { get; set; }
        public double LoadingCapacity { get; set; }
        public Variants Variants { get; set; }
    }
}