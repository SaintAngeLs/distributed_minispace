using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Customers.Application.Events.Rejected
{
    [Contract]
    public class CompleteCustomerRegistrationRejected : IRejectedEvent
    {
        public Guid CustomerId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CompleteCustomerRegistrationRejected(Guid customerId, string reason, string code)
        {
            CustomerId = customerId;
            Reason = reason;
            Code = code;
        }
    }
}