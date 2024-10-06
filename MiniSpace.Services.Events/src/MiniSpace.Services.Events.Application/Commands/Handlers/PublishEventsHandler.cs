using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class PublishEventsHandler : ICommandHandler<PublishEvents>
    {
        private readonly IEventRepository _eventRepository;

        public PublishEventsHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task HandleAsync(PublishEvents command, CancellationToken cancellationToken)
        {
            var eventsToPublish = await _eventRepository.GetEventsToBePublishedAsync(command.Now);
            foreach (var eventToPublish in eventsToPublish)
            {
                eventToPublish.UpdateState(command.Now);
                await _eventRepository.UpdateAsync(eventToPublish);
            }
        }
    }
}