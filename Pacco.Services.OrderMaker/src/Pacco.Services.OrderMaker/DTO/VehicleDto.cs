using System;

namespace Pacco.Services.OrderMaker.DTO
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal PricePerService { get; set; }
    }
}