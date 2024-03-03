using Pacco.Services.Pricing.Api.Core.Entities;

namespace Pacco.Services.Pricing.Api.Core.Services
{
    public class CustomerDiscountsService : ICustomerDiscountsService
    {
        public decimal CalculateDiscount(Customer customer)
        {
            var discount = 0.0m;

            if (customer.CompletedOrdersNumber >= 10)
            {
                discount = 0.1m;
            }
            else if (customer.CompletedOrdersNumber < 10 && customer.CompletedOrdersNumber > 3)
            {
                discount = 0.05m;
            }
            else if(customer.CompletedOrdersNumber <= 3 && customer.CompletedOrdersNumber > 0)
            {
                discount = 0.02m;
            }

            if (customer.IsVip)
            {
                discount += 0.1m;
            }

            return discount;
        }
    }
}