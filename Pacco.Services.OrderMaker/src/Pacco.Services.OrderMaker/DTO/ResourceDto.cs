using System;
using System.Collections.Generic;

namespace Pacco.Services.OrderMaker.DTO
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}