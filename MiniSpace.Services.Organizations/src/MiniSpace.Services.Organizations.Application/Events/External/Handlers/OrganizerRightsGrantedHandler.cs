using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Events.External.Handlers
{
    public class OrganizerRightsGrantedHandler : IEventHandler<OrganizerRightsGranted>
    {
        private readonly IOrganizerRepository _organizerRepository;
        
        public OrganizerRightsGrantedHandler(IOrganizerRepository organizerRepository)
        {
            _organizerRepository = organizerRepository;
        }
        
        public async Task HandleAsync(OrganizerRightsGranted @event, CancellationToken cancellationToken)
        {
            if (await _organizerRepository.ExistsAsync(@event.UserId))
            {
                throw new OrganizerAlreadyAddedException(@event.UserId);
            }

            await _organizerRepository.AddAsync(new Organizer(@event.UserId));
        }
    }
}