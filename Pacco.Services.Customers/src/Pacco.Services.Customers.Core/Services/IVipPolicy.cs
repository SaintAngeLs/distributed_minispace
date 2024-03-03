using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Core.Services
{
    public interface IVipPolicy
    {
        void ApplyVipStatusIfEligible(Customer customer);
    }
}