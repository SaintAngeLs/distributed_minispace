using System;

namespace Pacco.Services.Parcels.Application.Exceptions
{
    public class CustomerAlreadyExistsException : AppException
    {
        public override string Code { get; } = "customer_already_exists";
        public Guid CustomerId { get; }

        public CustomerAlreadyExistsException(Guid customerId) 
            : base($"Customer with id: {customerId} already exists.")
        {
            CustomerId = customerId;
        }
    }
}