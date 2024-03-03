using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Core.Events
{
    public class CustomerRegistrationCompleted : IDomainEvent
    {
        public Customer Customer { get; }

        public CustomerRegistrationCompleted(Customer customer)
        {
            Customer = customer;
        }
    }
}