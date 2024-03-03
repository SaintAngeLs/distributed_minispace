using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderForReservedVehicleNotFoundException : AppException
    {
        public override string Code { get; } = "order_for_reserved_vehicle_not_found";
        public Guid VehicleId { get; }
        public DateTime Date { get; }

        public OrderForReservedVehicleNotFoundException(Guid vehicleId, DateTime date) : base(
            $"Order for reserved vehicle: {vehicleId} for date: {date} was not found.")
        {
            VehicleId = vehicleId;
            Date = date;
        }
    }
}