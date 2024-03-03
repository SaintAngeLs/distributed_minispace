using System;
using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Core.Exceptions
{
    public class CannotChangeDeliveryStateException : DomainException
    {
        public override string Code { get; } = "cannot_change_delivery_state";

        public CannotChangeDeliveryStateException(Guid id, DeliveryStatus currentStatus, DeliveryStatus nextStatus) :
            base($"Cannot change state for delivery with id: '{id}' from {currentStatus} to {nextStatus}'")
        {
        }
    }
}