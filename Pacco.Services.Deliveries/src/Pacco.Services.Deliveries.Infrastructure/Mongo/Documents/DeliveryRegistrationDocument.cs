using System;

namespace Pacco.Services.Deliveries.Infrastructure.Mongo.Documents
{
    public class DeliveryRegistrationDocument
    {
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}