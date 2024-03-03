using System;

namespace Pacco.Services.Customers.Core.Exceptions
{
    public class InvalidCustomerFullNameException : DomainException
    {
        public override string Code { get; } = "invalid_customer_fullname";
        public Guid Id { get; }
        public string FullName { get; }

        public InvalidCustomerFullNameException(Guid id, string fullName) : base(
            $"Customer with id: {id} has invalid full name.")
        {
            Id = id;
            FullName = fullName;
        }
    }
}