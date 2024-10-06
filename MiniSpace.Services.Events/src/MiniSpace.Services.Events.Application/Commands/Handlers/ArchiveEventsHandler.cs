using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class ArchiveEventsHandler : ICommandHandler<ArchiveEvents>
    {
        private readonly IEventRepository _eventRepository;

        public ArchiveEventsHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task HandleAsync(ArchiveEvents command, CancellationToken cancellationToken)
        {
            var eventsToArchive = await _eventRepository.GetEventsToArchiveAsync(command.Now);
            foreach (var eventToArchive in eventsToArchive)
            {
                eventToArchive.UpdateState(command.Now);
                await _eventRepository.UpdateAsync(eventToArchive);
            }
        }
    }
}