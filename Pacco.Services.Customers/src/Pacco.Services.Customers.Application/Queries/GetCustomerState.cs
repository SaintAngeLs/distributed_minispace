using System;
using Convey.CQRS.Queries;
using Pacco.Services.Customers.Application.DTO;

namespace Pacco.Services.Customers.Application.Queries
{
    public class GetCustomerState : IQuery<CustomerStateDto>
    {
        public Guid CustomerId { get; set; }
    }
}