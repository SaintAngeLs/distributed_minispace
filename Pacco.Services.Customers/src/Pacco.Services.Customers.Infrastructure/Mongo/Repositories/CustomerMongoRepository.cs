using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Customers.Core.Entities;
using Pacco.Services.Customers.Core.Repositories;
using Pacco.Services.Customers.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Customers.Infrastructure.Mongo.Repositories
{
    public class CustomerMongoRepository : ICustomerRepository
    {
        private readonly IMongoRepository<CustomerDocument, Guid> _repository;

        public CustomerMongoRepository(IMongoRepository<CustomerDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            var customer = await _repository.GetAsync(o => o.Id == id);

            return customer?.AsEntity();
        }

        public Task AddAsync(Customer customer) => _repository.AddAsync(customer.AsDocument());
        public Task UpdateAsync(Customer customer) => _repository.UpdateAsync(customer.AsDocument());
    }
}