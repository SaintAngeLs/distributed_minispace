using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.Rejected
{
    public class AssignVehicleToOrderRejected : IRejectedEvent
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }
        public string Reason { get; }
        public string Code { get; }

        public AssignVehicleToOrderRejected(Guid orderId, Guid vehicleId, string reason, string code)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
            Reason = reason;
            Code = code;
        }
    }
}