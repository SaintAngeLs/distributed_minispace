using System;

namespace Pacco.Services.Parcels.Core.Exceptions
{
    public class CannotDeleteParcelException : DomainException
    {
        public override string Code { get; } = "cannot_delete_parcel";
        public Guid Id { get; }
        
        public CannotDeleteParcelException(Guid id) : base($"Parcel with id: '{id}' cannot be deleted.")
        {
            Id = id;
        }
    }
}