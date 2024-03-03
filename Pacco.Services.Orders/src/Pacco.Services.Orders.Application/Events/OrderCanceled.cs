using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderCanceled : IEvent
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public OrderCanceled(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }
    }
}