using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Parcels.Application.Events.External
{
    [Message("orders")]
    public class OrderDeleted : IEvent
    {
        public Guid OrderId { get; }

        public OrderDeleted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}