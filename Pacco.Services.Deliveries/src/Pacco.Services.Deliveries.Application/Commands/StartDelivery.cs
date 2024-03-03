using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Deliveries.Application.Commands
{
    [Contract]
    public class StartDelivery : ICommand
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string Description { get; }
        public DateTime DateTime { get; }

        public StartDelivery(Guid deliveryId, Guid orderId, string description, DateTime dateTime)
        {
            DeliveryId = deliveryId == Guid.Empty ? Guid.NewGuid() : deliveryId;
            OrderId = orderId;
            Description = description;
            DateTime = dateTime;
        }
    }
}