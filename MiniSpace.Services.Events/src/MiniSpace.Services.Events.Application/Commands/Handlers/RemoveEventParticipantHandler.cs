﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class RemoveEventParticipantHandler : ICommandHandler<RemoveEventParticipant>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public RemoveEventParticipantHandler(IEventRepository eventRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(RemoveEventParticipant command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var identity = _appContext.Identity;
            var isOrganizer = @event.Organizer.OrganizerType == OrganizerType.User 
                ? @event.Organizer.UserId == identity.Id 
                : @event.Organizer.OrganizationId == identity.Id;

            if (identity.IsAuthenticated && !isOrganizer && !identity.IsAdmin)
            {
                throw new UnauthorizedEventAccessException(@event.Id, identity.Id);
            }
            
            @event.RemoveParticipant(command.ParticipantId);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new EventParticipantRemoved(@event.Id, command.ParticipantId));
        }
    }
}
