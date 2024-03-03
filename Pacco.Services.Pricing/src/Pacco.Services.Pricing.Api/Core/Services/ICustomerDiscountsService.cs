using Pacco.Services.Pricing.Api.Core.Entities;

namespace Pacco.Services.Pricing.Api.Core.Services
{
    public interface ICustomerDiscountsService
    {
        decimal CalculateDiscount(Customer customer);
    }
}