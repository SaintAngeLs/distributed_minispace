using System;
using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Core.Exceptions
{
    public class CannotAddDeliveryRegistrationException : DomainException
    {
        public override string Code { get; } = "cannot_add_delivery_registration";

        public CannotAddDeliveryRegistrationException(Guid id, DeliveryStatus currentStatus) :
            base($"Cannot add registration to delivery with id: '{id}' and status {currentStatus}'")
        {
        }
    }
}