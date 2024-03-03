using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Customers.Application.Events
{
    [Contract]
    public class CustomerCreated : IEvent
    {
        public Guid CustomerId { get; }

        public CustomerCreated(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}