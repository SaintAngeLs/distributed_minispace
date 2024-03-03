using System;

namespace Pacco.Services.Pricing.Api.Exceptions
{
    public class CustomerNotFoundException : AppException
    {
        public override string Code { get; } = "customer_not_found";

        public CustomerNotFoundException(Guid id) : base($"Customer not found: {id}.")
        {
        }
    }
}