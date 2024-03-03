using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Deliveries.Core.Entities;
using Pacco.Services.Deliveries.Core.Repositories;
using Pacco.Services.Deliveries.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Deliveries.Infrastructure.Mongo.Repositories
{
    internal class DeliveriesMongoRepository : IDeliveriesRepository
    {
        private readonly IMongoRepository<DeliveryDocument, Guid> _repository;

        public DeliveriesMongoRepository(IMongoRepository<DeliveryDocument, Guid> repository)
            => _repository = repository;

        public async Task<Delivery> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(d => d.Id == id);
            return document?.AsEntity();
        }

        public async Task<Delivery> GetForOrderAsync(Guid id)
        {
            var document = await _repository.GetAsync(d => d.OrderId == id);
            return document?.AsEntity();
        }

        public Task AddAsync(Delivery delivery)
            => _repository.AddAsync(delivery.AsDocument());
        
        public Task UpdateAsync(Delivery delivery)
            => _repository.UpdateAsync(delivery.AsDocument());

        public Task DeleteAsync(Delivery delivery)
            => _repository.DeleteAsync(delivery.Id);
    }
}