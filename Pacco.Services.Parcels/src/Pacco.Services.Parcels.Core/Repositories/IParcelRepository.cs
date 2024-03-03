using System;
using System.Threading.Tasks;
using Pacco.Services.Parcels.Core.Entities;

namespace Pacco.Services.Parcels.Core.Repositories
{
    public interface IParcelRepository
    {
        Task<Parcel> GetAsync(Guid id);
        Task<Parcel> GetByOrderAsync(Guid orderId);
        Task AddAsync(Parcel parcel);
        Task UpdateAsync(Parcel parcel);
        Task DeleteAsync(Guid id);
    }
}