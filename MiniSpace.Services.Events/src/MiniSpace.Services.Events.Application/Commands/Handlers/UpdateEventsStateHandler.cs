using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class UpdateEventsStateHandler : ICommandHandler<UpdateEventsState>
    {
        private IEventRepository _eventRepository;
        private IMessageBroker _messageBroker;

        public UpdateEventsStateHandler(IEventRepository eventRepository, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateEventsState command, CancellationToken cancellationToken)
        {
            var events = (await _eventRepository.GetAllAsync()).ToList();
            foreach (var @event in events)
            {
                if (@event.UpdateState(command.Now))
                    await _eventRepository.UpdateAsync(@event);
            }
            
            await _messageBroker.PublishAsync(new EventsStateUpdated(command.Now));
        }
    }
}