using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Customers.Application.DTO;
using Pacco.Services.Customers.Application.Queries;
using Pacco.Services.Customers.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Customers.Infrastructure.Mongo.Queries.Handlers
{
    public class GetCustomerStateHandler : IQueryHandler<GetCustomerState, CustomerStateDto>
    {
        private readonly IMongoRepository<CustomerDocument, Guid> _customerRepository;

        public GetCustomerStateHandler(IMongoRepository<CustomerDocument, Guid> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerStateDto> HandleAsync(GetCustomerState query)
        {
            var document = await _customerRepository.GetAsync(p => p.Id == query.CustomerId);

            return document is null
                ? null
                : new CustomerStateDto
                {
                    Id = document.Id,
                    State = document.State.ToString().ToLowerInvariant()
                };
        }
    }
}