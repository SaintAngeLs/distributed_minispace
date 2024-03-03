using System;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CannotChangeOrderStateException : DomainException
    {
        public override string Code { get; } = "cannot_change_order_state";
        public Guid OrderId { get; }
        public OrderStatus CurrentStatus { get; }
        public OrderStatus NextStatus { get; }


        public CannotChangeOrderStateException(Guid orderId, OrderStatus currentStatus, OrderStatus nextStatus) :
            base($"Cannot change state for order with id: '{orderId}' from {currentStatus} to {nextStatus}'")
        {
            OrderId = orderId;
            CurrentStatus = currentStatus;
            NextStatus = nextStatus;
        }
    }
}