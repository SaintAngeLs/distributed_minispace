using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class InvalidOrderPriceException : DomainException
    {
        public override string Code { get; } = "invalid_order_price";

        public InvalidOrderPriceException(Guid id, decimal price)
            : base($"Order with id: '{id}' has an invalid price: {price}")
        {
        }
    }
}