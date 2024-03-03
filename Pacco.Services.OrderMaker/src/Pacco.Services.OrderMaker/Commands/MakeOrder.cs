using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.OrderMaker.Commands
{
    public class MakeOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public Guid ParcelId { get; }

        public MakeOrder(Guid orderId, Guid customerId, Guid parcelId)
        {
            OrderId = orderId == Guid.Empty ? Guid.NewGuid() : orderId;
            CustomerId = customerId;
            ParcelId = parcelId;
        }
    }
}