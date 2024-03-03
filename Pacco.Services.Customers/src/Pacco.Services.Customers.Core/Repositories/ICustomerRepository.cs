using System;
using System.Threading.Tasks;
using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetAsync(Guid id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
    }
}