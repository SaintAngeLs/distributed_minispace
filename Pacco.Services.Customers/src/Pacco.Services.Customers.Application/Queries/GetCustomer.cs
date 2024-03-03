using System;
using Convey.CQRS.Queries;
using Pacco.Services.Customers.Application.DTO;

namespace Pacco.Services.Customers.Application.Queries
{
    public class GetCustomer : IQuery<CustomerDetailsDto>
    {
        public Guid CustomerId { get; set; }
    }
}