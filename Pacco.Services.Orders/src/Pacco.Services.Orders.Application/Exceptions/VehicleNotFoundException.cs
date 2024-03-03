using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class VehicleNotFoundException : AppException
    {
        public override string Code { get; } = "vehicle_not_found";
        public Guid Id { get; }

        public VehicleNotFoundException(Guid id) : base($"Vehicle with id: {id} was not found.")
        {
            Id = id;
        }
    }
}