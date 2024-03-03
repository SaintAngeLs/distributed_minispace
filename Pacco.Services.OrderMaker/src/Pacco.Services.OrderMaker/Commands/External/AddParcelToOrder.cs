using System;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;

namespace Pacco.Services.OrderMaker.Commands.External
{
    [Message("orders")]
    public class AddParcelToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }
        public Guid CustomerId { get; }

        public AddParcelToOrder(Guid orderId, Guid parcelId, Guid customerId)
        {
            OrderId = orderId;
            ParcelId = parcelId;
            CustomerId = customerId;
        }
    }
}