using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class ParcelDeletedFromOrder : IEvent
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }

        public ParcelDeletedFromOrder(Guid orderId, Guid parcelId)
        {
            OrderId = orderId;
            ParcelId = parcelId;
        }
    }
}