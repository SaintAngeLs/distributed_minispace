using Pacco.Services.Deliveries.Core.Entities;
using Pacco.Services.Deliveries.Core.ValueObjects;

namespace Pacco.Services.Deliveries.Core.Events
{
    public class DeliveryRegistrationAdded : IDomainEvent
    {
        public Delivery Delivery { get; }
        public DeliveryRegistration Registration { get; }

        public DeliveryRegistrationAdded(Delivery delivery, DeliveryRegistration registration)
        {
            Delivery = delivery;
            Registration = registration;
        }
    }
}