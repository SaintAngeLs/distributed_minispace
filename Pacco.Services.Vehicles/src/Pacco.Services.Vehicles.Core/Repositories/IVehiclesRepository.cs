using System;
using System.Threading.Tasks;
using Pacco.Services.Vehicles.Core.Entities;

namespace Pacco.Services.Vehicles.Core.Repositories
{
    public interface IVehiclesRepository
    {
        Task<Vehicle> GetAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
    }
}