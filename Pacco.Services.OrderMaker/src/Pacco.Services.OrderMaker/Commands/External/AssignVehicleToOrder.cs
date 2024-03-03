using System;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Commands.External
{
    [Message("orders")]
    public class AssignVehicleToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }
        public DateTime DeliveryDate { get; }

        public AssignVehicleToOrder(Guid orderId, Guid vehicleId, DateTime deliveryDate)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
            DeliveryDate = deliveryDate;
        }
    }
}