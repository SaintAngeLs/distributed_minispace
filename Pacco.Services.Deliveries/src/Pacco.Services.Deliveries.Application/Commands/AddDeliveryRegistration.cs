using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Deliveries.Application.Commands
{
    [Contract]
    public class AddDeliveryRegistration : ICommand
    {
        public Guid DeliveryId { get; }
        public string Description { get; }
        public DateTime DateTime { get; }

        public AddDeliveryRegistration(Guid deliveryId, string description, DateTime dateTime)
        {
            DeliveryId = deliveryId;
            Description = description;
            DateTime = dateTime;
        }
    }
}