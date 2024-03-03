using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class OrderParcelNotFoundException : DomainException
    {
        public override string Code { get; } = "order_parcel_not_found";
        public Guid ParcelId { get; }
        public Guid OrderId { get; }

        public OrderParcelNotFoundException(Guid parcelId, Guid orderId) :
            base($"Parcel with id: {parcelId} was not found in order with id: {orderId}.")
        {
            ParcelId = parcelId;
            OrderId = orderId;
        }
    }
}