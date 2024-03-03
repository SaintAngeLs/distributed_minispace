using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Customers.Application.Commands
{
    [Contract]
    public class CompleteCustomerRegistration : ICommand
    {
        public Guid CustomerId { get; }
        public string FullName { get; }
        public string Address { get; }

        public CompleteCustomerRegistration(Guid customerId, string fullName, string address)
        {
            CustomerId = customerId;
            FullName = fullName;
            Address = address;
        }
    }
}