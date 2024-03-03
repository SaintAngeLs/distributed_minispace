using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Application.DTO
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Notes { get; set; }
        
        public DateTime? LastUpdate => Registrations
            .OrderByDescending(r => r.DateTime)
            .Select(r => r.DateTime)
            .FirstOrDefault();
        
        public IEnumerable<DeliveryRegistrationDto> Registrations { get; set; }
    }
}