using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class CustomerAlreadyAddedException : AppException
    {
        public override string Code { get; } = "customer_already_added";
        public Guid CustomerId { get; }

        public CustomerAlreadyAddedException(Guid customerId)
            : base($"Customer with id: {customerId} was already added.")
        {
            CustomerId = customerId;
        }
    }
}