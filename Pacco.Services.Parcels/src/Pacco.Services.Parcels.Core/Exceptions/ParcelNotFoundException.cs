using System;

namespace Pacco.Services.Parcels.Core.Exceptions
{
    public class ParcelNotFoundException : DomainException
    {
        public override string Code { get; } = "parcel_not_found";

        public ParcelNotFoundException(Guid id) : base($"Parcel with id: {id} was not found.")
        {
        }
    }
}