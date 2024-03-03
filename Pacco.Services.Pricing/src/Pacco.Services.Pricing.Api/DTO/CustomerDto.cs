using System;
using System.Collections.Generic;

namespace Pacco.Services.Pricing.Api.DTO
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public bool IsVip { get; set; }
        public IEnumerable<Guid> CompletedOrders { get; set; }
    }
}