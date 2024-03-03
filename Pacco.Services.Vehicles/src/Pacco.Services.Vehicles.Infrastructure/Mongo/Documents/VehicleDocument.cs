using System;
using Convey.Types;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Infrastructure.Mongo.Documents
{
    internal class VehicleDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public double PayloadCapacity { get; set; }
        public double LoadingCapacity { get; set; }
        public decimal PricePerService { get; set; }
        public Variants Variants { get; set; }
    }
}