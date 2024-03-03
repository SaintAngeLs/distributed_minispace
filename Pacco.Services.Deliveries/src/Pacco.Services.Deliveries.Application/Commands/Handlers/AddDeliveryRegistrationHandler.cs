using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Deliveries.Application.Exceptions;
using Pacco.Services.Deliveries.Application.Services;
using Pacco.Services.Deliveries.Core.Repositories;
using Pacco.Services.Deliveries.Core.ValueObjects;

namespace Pacco.Services.Deliveries.Application.Commands.Handlers
{
    internal sealed class AddDeliveryRegistrationHandler : ICommandHandler<AddDeliveryRegistration>
    {
        private readonly IDeliveriesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddDeliveryRegistrationHandler(IDeliveriesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(AddDeliveryRegistration command)
        {
            var delivery = await  _repository.GetAsync(command.DeliveryId);
            if (delivery is null)
            {
                throw new DeliveryNotFoundException(command.DeliveryId);
            }
            
            delivery.AddRegistration(new DeliveryRegistration(command.Description, command.DateTime));
            if (delivery.HasChanged)
            {
                await _repository.UpdateAsync(delivery);
                var events = _eventMapper.MapAll(delivery.Events);
                await _messageBroker.PublishAsync(events.ToArray());
            }
        }
    }
}