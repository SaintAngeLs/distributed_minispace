using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IEventRepository _eventRepository;

        public EventDeletedHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public async Task HandleAsync(EventDeleted @event, CancellationToken cancellationToken = default)
        {
            if (!(await _eventRepository.ExistsAsync(@event.EventId)))
            {
                throw new EventNotFoundException(@event.EventId);
            }

            await _eventRepository.DeleteAsync(@event.EventId);
        }
    }    
}
