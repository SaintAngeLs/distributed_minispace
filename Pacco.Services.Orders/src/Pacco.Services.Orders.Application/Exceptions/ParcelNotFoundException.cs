using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class ParcelNotFoundException : AppException
    {
        public override string Code { get; } = "parcel_not_found";
        public Guid ParcelId { get; }

        public ParcelNotFoundException(Guid parcelId) : base($"Parcel with id: {parcelId} was not found.")
        {
            ParcelId = parcelId;
        }
    }
}