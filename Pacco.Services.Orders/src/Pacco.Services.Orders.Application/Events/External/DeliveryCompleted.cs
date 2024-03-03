using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [Message("deliveries")]
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