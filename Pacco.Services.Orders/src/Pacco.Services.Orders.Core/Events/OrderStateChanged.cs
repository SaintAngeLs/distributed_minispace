using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Events
{
    public class OrderStateChanged : IDomainEvent
    {
        public Order Order { get; }

        public OrderStateChanged(Order order)
        {
            Order = order;
        }
    }
}