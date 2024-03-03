using System;
using System.Collections.Generic;

namespace Pacco.Services.Vehicles.Application.DTO
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public double PayloadCapacity { get; set; }
        public double LoadingCapacity { get; set; }
        public decimal PricePerService { get; set; }
        public IEnumerable<string> Variants { get; set; }
    }
}