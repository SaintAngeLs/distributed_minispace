using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Orders.Application.DTO;

namespace Pacco.Services.Orders.Application.Queries
{
    public class GetOrders : IQuery<IEnumerable<OrderDto>>
    {
        public Guid? CustomerId { get; set; }
    }
}