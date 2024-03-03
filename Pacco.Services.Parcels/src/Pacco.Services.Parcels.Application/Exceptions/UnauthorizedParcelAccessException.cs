using System;

namespace Pacco.Services.Parcels.Application.Exceptions
{
    public class UnauthorizedParcelAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_parcel_access";

        public UnauthorizedParcelAccessException(Guid id, Guid customerId) 
            : base($"Unauthorized access to parcel: '{id}' by customer: '{customerId}'")
        {
        }
    }
}