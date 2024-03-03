using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
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