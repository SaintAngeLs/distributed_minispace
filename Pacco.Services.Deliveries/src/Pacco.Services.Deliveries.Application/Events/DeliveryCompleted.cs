using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Deliveries.Application.Events
{
    [Contract]
    public class DeliveryCompleted : IEvent
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }

        public DeliveryCompleted(Guid deliveryId, Guid orderId)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
        }
    }
}