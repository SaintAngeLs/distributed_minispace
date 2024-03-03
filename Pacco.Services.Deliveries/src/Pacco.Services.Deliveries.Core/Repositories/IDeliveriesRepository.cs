using System;
using System.Threading.Tasks;
using Pacco.Services.Deliveries.Core.Entities;

namespace Pacco.Services.Deliveries.Core.Repositories
{
    public interface IDeliveriesRepository
    {
        Task<Delivery> GetAsync(Guid id);
        Task<Delivery> GetForOrderAsync(Guid id);
        Task AddAsync(Delivery delivery);
        Task UpdateAsync(Delivery delivery);
        Task DeleteAsync(Delivery delivery);
    }
}