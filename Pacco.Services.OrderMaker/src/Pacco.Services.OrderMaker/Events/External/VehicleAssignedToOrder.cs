using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Events.External
{
    [Message("orders")]
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