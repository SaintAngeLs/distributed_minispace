using System.Threading.Tasks;
using Pacco.Services.Vehicles.Application.DTO;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static Vehicle AsEntity(this VehicleDocument document)
            => document is null? null : new Vehicle(
                document.Id,
                document.Brand,
                document.Model,
                document.Description,
                document.PayloadCapacity,
                document.LoadingCapacity,
                document.PricePerService,
                document.Variants);

        public static async Task<Vehicle> AsEntityAsync(this Task<VehicleDocument> task)
            => (await task).AsEntity();

        public static VehicleDocument AsDocument(this Vehicle entity)
            => new VehicleDocument
            {
                Id = entity.Id,
                Brand = entity.Brand,
                Model = entity.Model,
                Description = entity.Description,
                PayloadCapacity = entity.PayloadCapacity,
                LoadingCapacity = entity.LoadingCapacity,
                PricePerService = entity.PricePerService,
                Variants = entity.Variants
            };
        
        public static async Task<VehicleDocument> AsDocumentAsync(this Task<Vehicle> task)
            => (await task).AsDocument();

        public static VehicleDto AsDto(this VehicleDocument document)
            => new VehicleDto
            {
                Id = document.Id,
                Brand = document.Brand,
                Model = document.Model,
                Description = document.Description,
                PayloadCapacity = document.PayloadCapacity,
                LoadingCapacity = document.LoadingCapacity,
                PricePerService = document.PricePerService,
                Variants = document.Variants.ToString().Split(',')
            };
    }
}