using System;

namespace Pacco.Services.Vehicles.Application.Exceptions
{
    public class VehicleNotFoundException : AppException
    {
        public override string Code { get; } = "vehicle_not_found";
        public Guid Id { get; }

        public VehicleNotFoundException(Guid id) : base($"Vehicle not found: {id}.")
        {
            Id = id;
        }
    }
}