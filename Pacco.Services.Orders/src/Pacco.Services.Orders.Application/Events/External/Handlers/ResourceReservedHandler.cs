using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class ResourceReservedHandler : IEventHandler<ResourceReserved>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ResourceReservedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ResourceReserved @event)
        {
            var order = await _orderRepository.GetAsync(@event.ResourceId, @event.DateTime);
            if (order is null)
            {
                throw new OrderForReservedVehicleNotFoundException(@event.ResourceId, @event.DateTime);
            }

            order.Approve();
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}