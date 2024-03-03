using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderNotFoundException : AppException
    {
        public override string Code { get; } = "order_not_found";
        public Guid Id { get; }
        
        public OrderNotFoundException(Guid id) : base($"Order with id: {id} was not found.")
        {
            Id = id;
        }
    }
}