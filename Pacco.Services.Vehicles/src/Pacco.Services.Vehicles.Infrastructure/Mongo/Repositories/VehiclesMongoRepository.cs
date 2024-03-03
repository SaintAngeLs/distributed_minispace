using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Vehicles.Core.Entities;
using Pacco.Services.Vehicles.Core.Repositories;
using Pacco.Services.Vehicles.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Vehicles.Infrastructure.Mongo.Repositories
{
    internal class VehiclesMongoRepository : IVehiclesRepository
    {
        private readonly IMongoRepository<VehicleDocument, Guid> _repository;

        public VehiclesMongoRepository(IMongoRepository<VehicleDocument, Guid> repository)
            => _repository = repository;

        public Task<Vehicle> GetAsync(Guid id)
            => _repository
                .GetAsync(id)
                .AsEntityAsync();

        public Task AddAsync(Vehicle vehicle)
            => _repository.AddAsync(vehicle.AsDocument());

        public Task UpdateAsync(Vehicle vehicle)
            => _repository.UpdateAsync(vehicle.AsDocument());

        public Task DeleteAsync(Vehicle vehicle)
            => _repository.DeleteAsync(vehicle.Id);
    }
}