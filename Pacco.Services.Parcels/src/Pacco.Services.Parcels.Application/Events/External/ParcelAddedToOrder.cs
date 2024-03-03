using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Parcels.Application.Events.External
{
    [Message("orders")]
    public class ParcelAddedToOrder : IEvent
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }

        public ParcelAddedToOrder(Guid orderId, Guid parcelId)
        {
            OrderId = orderId;
            ParcelId = parcelId;
        }
    }
}