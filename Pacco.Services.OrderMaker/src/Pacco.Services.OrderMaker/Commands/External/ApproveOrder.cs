using System;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Commands.External
{
    [Message("orders")]
    public class ApproveOrder : ICommand
    {
        public Guid OrderId { get; }

        public ApproveOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}