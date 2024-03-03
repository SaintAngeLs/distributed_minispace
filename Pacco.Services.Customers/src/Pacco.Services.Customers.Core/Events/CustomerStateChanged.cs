using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Core.Events
{
    public class CustomerStateChanged : IDomainEvent
    {
        public Customer Customer { get; }
        public State PreviousState { get; }

        public CustomerStateChanged(Customer customer, State previousState)
        {
            Customer = customer;
            PreviousState = previousState;
        }
    }
}