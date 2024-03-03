using System;
using Convey.CQRS.Queries;
using Pacco.Services.Vehicles.Application.DTO;

namespace Pacco.Services.Vehicles.Application.Queries
{
    public class GetVehicle : IQuery<VehicleDto>
    {
        public Guid VehicleId { get; set; }
    }
}