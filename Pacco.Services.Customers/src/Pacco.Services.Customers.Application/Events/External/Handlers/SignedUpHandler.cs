using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Customers.Application.Exceptions;
using Pacco.Services.Customers.Application.Services;
using Pacco.Services.Customers.Core.Entities;
using Pacco.Services.Customers.Core.Repositories;

namespace Pacco.Services.Customers.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string RequiredRole = "user";
        private readonly ICustomerRepository _customerRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SignedUpHandler> _logger;

        public SignedUpHandler(ICustomerRepository customerRepository, IDateTimeProvider dateTimeProvider,
            ILogger<SignedUpHandler> logger)
        {
            _customerRepository = customerRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            if (@event.Role != RequiredRole)
            {
                throw new InvalidRoleException(@event.UserId, @event.Role, RequiredRole);
            }

            var customer = await _customerRepository.GetAsync(@event.UserId);
            if (customer is {})
            {
                throw new CustomerAlreadyCreatedException(customer.Id);
            }

            customer = new Customer(@event.UserId, @event.Email, _dateTimeProvider.Now);
            await _customerRepository.AddAsync(customer);
        }
    }
}