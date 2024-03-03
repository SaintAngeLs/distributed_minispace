using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;
namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCreatedHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task HandleAsync(CustomerCreated @event)
        {
            if (await _customerRepository.ExistsAsync(@event.CustomerId))
            {
                throw new CustomerAlreadyAddedException(@event.CustomerId);
            }

            await _customerRepository.AddAsync(new Customer(@event.CustomerId));
        }
    }
}