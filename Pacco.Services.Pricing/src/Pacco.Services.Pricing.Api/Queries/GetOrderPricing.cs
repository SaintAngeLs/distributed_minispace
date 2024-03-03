using System;
using Convey.CQRS.Queries;
using Pacco.Services.Pricing.Api.DTO;

namespace Pacco.Services.Pricing.Api.Queries
{
    public class GetOrderPricing : IQuery<OrderPricingDto>
    {
        public Guid CustomerId { get; set; }
        public decimal OrderPrice { get; set; }
    }
}