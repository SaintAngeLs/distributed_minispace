using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Events;

namespace Pacco.Services.Orders.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case OrderStateChanged e:
                    switch (e.Order.Status)
                    {
                        case OrderStatus.New:
                            return new OrderCreated(e.Order.Id);
                        case OrderStatus.Approved:
                            return new OrderApproved(e.Order.Id);
                        case OrderStatus.Delivering:
                            return new OrderDelivering(e.Order.Id);
                        case OrderStatus.Completed:
                            return new OrderCompleted(e.Order.Id, e.Order.CustomerId);
                        case OrderStatus.Canceled:
                            return new OrderCanceled(e.Order.Id, e.Order.CancellationReason);
                    }

                    break;
                case ParcelAdded e:
                    return new ParcelAddedToOrder(e.Order.Id, e.Parcel.Id);
                case ParcelDeleted e:
                    return new ParcelDeletedFromOrder(e.Order.Id, e.Parcel.Id);
            }

            return null;
        }
    }
}