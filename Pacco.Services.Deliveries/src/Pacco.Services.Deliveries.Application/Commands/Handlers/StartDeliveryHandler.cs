using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Deliveries.Application.Exceptions;
using Pacco.Services.Deliveries.Application.Services;
using Pacco.Services.Deliveries.Core.Entities;
using Pacco.Services.Deliveries.Core.Repositories;
using Pacco.Services.Deliveries.Core.ValueObjects;

namespace Pacco.Services.Deliveries.Application.Commands.Handlers
{
    internal sealed class StartDeliveryHandler : ICommandHandler<StartDelivery>
    {
        private readonly IDeliveriesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public StartDeliveryHandler(IDeliveriesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(StartDelivery command)
        {
            var delivery = await _repository.GetForOrderAsync(command.OrderId);
            if (delivery is {} && delivery.Status != DeliveryStatus.Failed)
            {
                throw new DeliveryAlreadyStartedException(command.OrderId);
            }

            delivery = Delivery.Create(command.DeliveryId, command.OrderId, DeliveryStatus.InProgress);
            delivery.AddRegistration(new DeliveryRegistration(command.Description, command.DateTime));
            await _repository.AddAsync(delivery);
            var events = _eventMapper.MapAll(delivery.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}