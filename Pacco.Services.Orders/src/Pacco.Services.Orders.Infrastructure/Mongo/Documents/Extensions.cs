using System.Linq;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Order AsEntity(this OrderDocument document)
            => new Order(document.Id, document.CustomerId, document.Status, document.CreatedAt,
                document.Parcels.Select(p => new Parcel(p.Id, p.Name, p.Variant, p.Size)),
                document.VehicleId, document.DeliveryDate, document.TotalPrice);

        public static OrderDocument AsDocument(this Order entity)
            => new OrderDocument
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                VehicleId = entity.VehicleId,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                DeliveryDate = entity.DeliveryDate,
                TotalPrice = entity.TotalPrice,
                Parcels = entity.Parcels.Select(p => new OrderDocument.Parcel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Variant = p.Variant
                })
            };

        public static OrderDto AsDto(this OrderDocument document)
            => new OrderDto
            {
                Id = document.Id,
                CustomerId = document.CustomerId,
                VehicleId = document.VehicleId,
                Status = document.Status.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
                DeliveryDate = document.DeliveryDate,
                TotalPrice = document.TotalPrice,
                Parcels = document.Parcels.Select(p => new ParcelDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Variant = p.Variant
                })
            };
        
        public static Customer AsEntity(this CustomerDocument document)
            => new Customer(document.Id);

        public static CustomerDocument AsDocument(this Customer entity)
            => new CustomerDocument
            {
                Id = entity.Id
            };
    }
}