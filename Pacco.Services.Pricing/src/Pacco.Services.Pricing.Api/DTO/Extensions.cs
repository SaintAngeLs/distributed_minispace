using System.Linq;
using Pacco.Services.Pricing.Api.Core.Entities;

namespace Pacco.Services.Pricing.Api.DTO
{
    internal static class Extensions
    {
        public static Customer AsEntity(this CustomerDto dto)
            => new Customer(dto.Id, dto.IsVip, dto.CompletedOrders.Count());
    }
}