using System;

namespace Pacco.Services.Deliveries.Application.Exceptions
{
    public class DeliveryAlreadyStartedException : AppException
    {
        public override string Code { get; } = "delivery_already_started";
        
        public DeliveryAlreadyStartedException(Guid orderId) : base($"Delivery for order: {orderId} was already started.")
        {
        }
    }
}