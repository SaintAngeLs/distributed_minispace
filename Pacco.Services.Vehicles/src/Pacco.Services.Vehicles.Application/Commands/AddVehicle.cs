using System;
using Convey.CQRS.Commands;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Application.Commands
{
    [Contract]
    public class AddVehicle : ICommand
    {
        public Guid VehicleId { get; }
        public string Brand { get; }
        public string Model { get; }
        public string Description { get; }
        public double PayloadCapacity { get; }
        public double LoadingCapacity { get; }
        public decimal PricePerService { get; }
        public Variants Variants { get; }
        
        public AddVehicle(Guid vehicleId, string brand, string model, string description, double payloadCapacity, 
            double loadingCapacity, decimal pricePerService, Variants variants)
        {
            VehicleId = vehicleId == Guid.Empty ? Guid.NewGuid() : vehicleId;
            Brand = brand;
            Model = model;
            Description = description;
            PayloadCapacity = payloadCapacity;
            LoadingCapacity = loadingCapacity;
            PricePerService = pricePerService;
            Variants = variants;
        }
    }
}