using System;
using Convey.CQRS.Events;

namespace Pacco.Services.OrderMaker.Events
{
    public class MakeOrderCompleted : IEvent
    {
        public Guid OrderId { get; }

        public MakeOrderCompleted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}