using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Deliveries.Application.Events
{
    [Contract]
    public class DeliveryFailed : IEvent
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string Reason { get; }

        public DeliveryFailed(Guid deliveryId, Guid orderId, string reason)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            Reason = reason;
        }
    }
}