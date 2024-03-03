using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Customers.Application.Events
{
    [Contract]
    public class CustomerStateChanged : IEvent
    {
        public Guid CustomerId { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public CustomerStateChanged(Guid customerId, string currentState, string previousState)
        {
            CustomerId = customerId;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }
}