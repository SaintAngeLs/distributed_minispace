using System;

namespace Pacco.Services.Orders.Application.DTO
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public decimal PricePerService { get; set; }
    }
}