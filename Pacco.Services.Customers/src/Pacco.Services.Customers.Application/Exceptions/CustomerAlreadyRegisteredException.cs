using System;

namespace Pacco.Services.Customers.Application.Exceptions
{
    public class CustomerAlreadyRegisteredException : AppException
    {
        public override string Code { get; } = "customer_already_registered";
        public Guid Id { get; }
        
        public CustomerAlreadyRegisteredException(Guid id) 
            : base($"Customer with id: {id} has already been registered.")
        {
            Id = id;
        }
    }
}