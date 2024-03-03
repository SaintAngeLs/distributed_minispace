using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class ParcelAlreadyAddedToOrderException : DomainException
    {
        public override string Code { get; } = "parcel_already_added_to_order";
        public Guid OrderId { get; }
        public Guid ParcelId { get; }

        public ParcelAlreadyAddedToOrderException(Guid orderId, Guid parcelId)
            : base($"Parcel with id: {parcelId} was already added to order: {orderId}.")
        {
            OrderId = orderId;
            ParcelId = parcelId;
        }
    }
}