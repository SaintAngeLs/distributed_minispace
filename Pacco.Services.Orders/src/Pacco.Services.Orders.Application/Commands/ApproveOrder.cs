using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class ApproveOrder : ICommand
    {
        public Guid OrderId { get; }

        public ApproveOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}