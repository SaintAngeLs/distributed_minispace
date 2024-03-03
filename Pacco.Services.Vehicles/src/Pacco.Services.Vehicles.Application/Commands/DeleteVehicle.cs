using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Vehicles.Application.Commands
{
    [Contract]
    public class DeleteVehicle : ICommand
    {
        public Guid VehicleId { get; }
        
        public DeleteVehicle(Guid id)
            => VehicleId = id;
    }
}