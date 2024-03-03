using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderApproved : IEvent
    {
        public Guid OrderId { get; }

        public OrderApproved(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}