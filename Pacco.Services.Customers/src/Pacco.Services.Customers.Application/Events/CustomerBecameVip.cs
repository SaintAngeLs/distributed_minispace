using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Customers.Application.Events
{
    [Contract]
    public class CustomerBecameVip : IEvent
    {
        public Guid CustomerId { get; }

        public CustomerBecameVip(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}