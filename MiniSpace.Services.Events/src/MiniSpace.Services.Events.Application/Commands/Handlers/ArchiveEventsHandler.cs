using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Core.Repositories;
using Paralax.CQRS.Events;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class ArchiveEventsHandler : ICommandHandler<ArchiveEvents>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageBroker _messageBroker;  

        public ArchiveEventsHandler(IEventRepository eventRepository, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(ArchiveEvents command, CancellationToken cancellationToken)
        {
            var eventsToArchive = await _eventRepository.GetEventsToArchiveAsync(command.Now);

            foreach (var eventToArchive in eventsToArchive)
            {
                eventToArchive.UpdateState(command.Now); 
                await _eventRepository.UpdateAsync(eventToArchive);  

                var eventArchived = new EventArchived(eventToArchive.Id);
                await _messageBroker.PublishAsync(eventArchived);  
            }
        }
    }
}
