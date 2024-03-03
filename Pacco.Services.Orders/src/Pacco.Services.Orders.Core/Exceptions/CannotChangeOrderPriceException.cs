using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CannotChangeOrderPriceException : DomainException
    {
        public override string Code { get; } = "cannot_change_order_price";
        public Guid OrderId { get; }

        public CannotChangeOrderPriceException(Guid orderId) :
            base($"Order with id: '{orderId}' cannot have a changed price.")
        {
            OrderId = orderId;
        }
    }
}