using System;
using System.Threading.Tasks;
using Pacco.Services.Parcels.Core.Entities;

namespace Pacco.Services.Parcels.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Customer customer);
    }
}