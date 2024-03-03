using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderCompleted : IEvent
    {
        public Guid OrderId { get; }
        public Guid CustomerId { get; }

        public OrderCompleted( Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}