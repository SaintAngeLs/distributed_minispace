using System.Linq;
using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Core.Services
{
    public class VipPolicy : IVipPolicy
    {
        public void ApplyVipStatusIfEligible(Customer customer)
        {
            if (customer.IsVip)
            {
                return;
            }

            if (customer.CompletedOrders.Count() < 20)
            {
                return;
            }
            
            customer.SetVip();
        }
    }
}