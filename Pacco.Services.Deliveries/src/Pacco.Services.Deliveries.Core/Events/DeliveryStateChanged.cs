using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Core.Events
{
    public class DeliveryStateChanged : IDomainEvent
    {
        public Delivery Delivery { get; }

        public DeliveryStateChanged(Delivery delivery)
        {
            Delivery = delivery;
        }
    }
}