using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class DeleteOrder : ICommand
    {
        public Guid OrderId { get; }

        public DeleteOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}