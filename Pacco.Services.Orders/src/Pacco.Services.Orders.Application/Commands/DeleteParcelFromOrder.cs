using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class DeleteParcelFromOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }

        public DeleteParcelFromOrder(Guid orderId, Guid parcelId)
        {
            OrderId = orderId;
            ParcelId = parcelId;
        }
    }
}