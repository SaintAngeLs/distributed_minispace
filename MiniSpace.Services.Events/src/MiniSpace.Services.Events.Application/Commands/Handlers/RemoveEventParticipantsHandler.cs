using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class RemoveEventParticipantsHandler: ICommandHandler<RemoveEventParticipants>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;
        private IMessageBroker _messageBroker;
        
        public RemoveEventParticipantsHandler(IEventRepository eventRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(RemoveEventParticipants command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if(@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && @event.Organizer.Id != identity.Id && !identity.IsAdmin)
            {
                throw new UnauthorizedEventAccessException(@event.Id, identity.Id);
            }
            @event.RemoveParticipants(command.Participants);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new EventParticipantsRemoved(@event.Id, command.Participants));
        }
        
    }
}