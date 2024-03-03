using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class VehicleAssignedToOrder : IEvent
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }

        public VehicleAssignedToOrder(Guid orderId, Guid vehicleId)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
        }
    }
}