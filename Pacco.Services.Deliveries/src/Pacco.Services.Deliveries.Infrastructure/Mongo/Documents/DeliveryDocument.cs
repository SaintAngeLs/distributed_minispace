using System;
using System.Collections.Generic;
using Convey.Types;
using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Infrastructure.Mongo.Documents
{
    public class DeliveryDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Notes { get; set; }
        public IEnumerable<DeliveryRegistrationDocument> Registrations { get; set; }
    }
}