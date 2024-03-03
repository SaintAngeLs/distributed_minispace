using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Deliveries.Application.Commands
{
    [Contract]
    public class CompleteDelivery : ICommand
    {
        public Guid DeliveryId { get; }

        public CompleteDelivery(Guid deliveryId)
            => DeliveryId = deliveryId;
    }
}