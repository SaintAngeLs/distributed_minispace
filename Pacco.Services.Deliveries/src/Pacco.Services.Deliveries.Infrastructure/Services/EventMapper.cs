using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Deliveries.Application.Events;
using Pacco.Services.Deliveries.Application.Services;
using Pacco.Services.Deliveries.Core.Entities;
using Pacco.Services.Deliveries.Core.Events;

namespace Pacco.Services.Deliveries.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case DeliveryStateChanged e:
                    switch (e.Delivery.Status)
                    {
                        case DeliveryStatus.InProgress:
                            return new DeliveryStarted(e.Delivery.Id, e.Delivery.OrderId);
                        case DeliveryStatus.Completed:
                            return new DeliveryCompleted(e.Delivery.Id, e.Delivery.OrderId);
                        case DeliveryStatus.Failed:
                            return new DeliveryFailed(e.Delivery.Id, e.Delivery.OrderId, e.Delivery.Notes);
                    }
                    break;
                case DeliveryRegistrationAdded e:
                    return new RegistrationAddedToDelivery(e.Delivery.Id, e.Delivery.OrderId, e.Registration.Description);
            }

            return null;
        }
    }
}