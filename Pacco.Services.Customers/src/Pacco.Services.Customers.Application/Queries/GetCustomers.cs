using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Customers.Application.DTO;

namespace Pacco.Services.Customers.Application.Queries
{
    public class GetCustomers : IQuery<IEnumerable<CustomerDto>>
    {
    }
}