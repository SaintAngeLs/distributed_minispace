using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderDeleted : IEvent
    {
        public Guid OrderId { get; }

        public OrderDeleted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}