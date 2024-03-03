using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Parcels.Application.Events.External
{
    [Message("orders")]
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