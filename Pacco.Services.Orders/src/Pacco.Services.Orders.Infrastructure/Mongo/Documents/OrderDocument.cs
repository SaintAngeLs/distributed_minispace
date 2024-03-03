using System;
using System.Collections.Generic;
using Convey.Types;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Infrastructure.Mongo.Documents
{
    public class OrderDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<Parcel> Parcels { get; set; }

        public class Parcel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Variant { get; set; }
            public string Size { get; set; }
        }
    }
}