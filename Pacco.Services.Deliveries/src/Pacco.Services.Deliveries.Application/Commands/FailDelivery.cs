using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Deliveries.Application.Commands
{
    [Contract]
    public class FailDelivery : ICommand
    {
        public Guid DeliveryId { get; }
        public string Reason { get; }

        public FailDelivery(Guid deliveryId, string reason)
        {
            DeliveryId = deliveryId;
            Reason = reason;
        }
    }
}