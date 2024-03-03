using System;

namespace Pacco.Services.Customers.Core.Exceptions
{
    public class InvalidCustomerAddressException : DomainException
    {
        public override string Code { get; } = "invalid_customer_address";
        public Guid Id { get; }
        public string Address { get; }

        public InvalidCustomerAddressException(Guid id, string address) : base(
            $"Customer with id: {id} has invalid address.")
        {
            Id = id;
            Address = address;
        }
    }
}