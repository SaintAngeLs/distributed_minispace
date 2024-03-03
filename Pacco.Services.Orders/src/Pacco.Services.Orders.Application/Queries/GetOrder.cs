using System;
using Convey.CQRS.Queries;
using Pacco.Services.Orders.Application.DTO;

namespace Pacco.Services.Orders.Application.Queries
{
    public class GetOrder : IQuery<OrderDto>
    {
        public Guid OrderId { get; set; }
    }
}