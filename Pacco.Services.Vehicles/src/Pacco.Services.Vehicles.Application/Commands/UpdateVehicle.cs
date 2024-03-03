using System;
using Convey.CQRS.Commands;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Application.Commands
{
    [Contract]
    public class UpdateVehicle : ICommand
    {
        public Guid VehicleId { get; }
        public string Description { get; }
        public decimal PricePerService { get; }
        public Variants Variants { get; }

        public UpdateVehicle(Guid vehicleId,string description, decimal pricePerService, Variants variants)
        {
            VehicleId = vehicleId;
            Description = description;
            PricePerService = pricePerService;
            Variants = variants;
        }
    }
}