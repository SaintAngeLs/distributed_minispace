using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderDelivering : IEvent
    {
        public Guid OrderId { get; }

        public OrderDelivering(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}