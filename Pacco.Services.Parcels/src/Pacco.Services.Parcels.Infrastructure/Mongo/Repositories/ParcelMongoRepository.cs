using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Parcels.Core.Entities;
using Pacco.Services.Parcels.Core.Repositories;
using Pacco.Services.Parcels.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Parcels.Infrastructure.Mongo.Repositories
{
    public class ParcelMongoRepository : IParcelRepository
    {
        private readonly IMongoRepository<ParcelDocument, Guid> _repository;

        public ParcelMongoRepository(IMongoRepository<ParcelDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Parcel> GetAsync(Guid id)
        {
            var parcel = await _repository.GetAsync(id);

            return parcel?.AsEntity();
        }

        public async Task<Parcel> GetByOrderAsync(Guid orderId)
        {
            var parcel = await _repository.GetAsync(p => p.OrderId == orderId);

            return parcel?.AsEntity();
        }

        public Task AddAsync(Parcel parcel) => _repository.AddAsync(parcel.AsDocument());
        public Task UpdateAsync(Parcel parcel) => _repository.UpdateAsync(parcel.AsDocument());
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}