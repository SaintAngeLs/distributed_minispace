using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Events.External
{
    [Message("orders")]
    public class OrderApproved : IEvent
    {
        public Guid OrderId { get; }

        public OrderApproved(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}