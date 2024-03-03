using System;
using System.Threading.Tasks;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Customer customer);
    }
}