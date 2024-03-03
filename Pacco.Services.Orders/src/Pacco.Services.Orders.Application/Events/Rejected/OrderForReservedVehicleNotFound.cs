using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.Rejected
{
    public class OrderForReservedVehicleNotFound : IRejectedEvent
    {
        public Guid VehicleId { get; }
        public DateTime Date { get; }
        public string Reason { get; }
        public string Code { get; }

        public OrderForReservedVehicleNotFound(Guid vehicleId, DateTime date, string reason, string code)
        {
            VehicleId = vehicleId;
            Date = date;
            Reason = reason;
            Code = code;
        }
    }
}