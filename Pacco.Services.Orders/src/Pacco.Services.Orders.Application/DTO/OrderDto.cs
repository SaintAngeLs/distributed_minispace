using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Application.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<ParcelDto> Parcels { get; set; }

        public OrderDto()
        {
        }

        public OrderDto(Order order)
        {
            Id = order.Id;
            CustomerId = order.CustomerId;
            VehicleId = order.VehicleId;
            Status = order.Status.ToString().ToLowerInvariant();
            CreatedAt = order.CreatedAt;
            DeliveryDate = order.DeliveryDate;
            TotalPrice = order.TotalPrice;
            Parcels = order.Parcels.Select(p => new ParcelDto(p));
        }
    }
}