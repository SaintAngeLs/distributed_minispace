using Paralax.CQRS.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Events.External.Handlers
{
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IEventRepository _eventRepository;

        public EventCreatedHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public async Task HandleAsync(EventCreated @event, CancellationToken cancellationToken = default)
        {
            if (await _eventRepository.ExistsAsync(@event.EventId))
            {
                throw new EventAlreadyAddedException(@event.EventId);
            }

            await _eventRepository.AddAsync(new Event(@event.EventId));
        }
    }    
}