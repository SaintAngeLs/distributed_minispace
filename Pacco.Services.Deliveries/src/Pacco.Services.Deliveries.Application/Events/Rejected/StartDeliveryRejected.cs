using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Deliveries.Application.Events.Rejected
{
    [Contract]
    public class StartDeliveryRejected : IRejectedEvent
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string Reason { get; }
        public string Code { get; }

        public StartDeliveryRejected(Guid deliveryId, Guid orderId, string reason, string code)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            Reason = reason;
            Code = code;
        }
    }
}