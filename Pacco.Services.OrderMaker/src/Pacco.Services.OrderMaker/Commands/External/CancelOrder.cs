using System;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Commands.External
{
    [Message("orders")]
    public class CancelOrder : ICommand
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public CancelOrder(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }
    }
}