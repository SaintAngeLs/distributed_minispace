using System;

namespace Pacco.Services.Deliveries.Application.Exceptions
{
    public class DeliveryNotFoundException : AppException
    {
        public override string Code { get; } = "delivery_not_found";

        public DeliveryNotFoundException(Guid deliveryId) : base($"Delivery with id: {deliveryId} was not found.")
        {
        }
    }
}