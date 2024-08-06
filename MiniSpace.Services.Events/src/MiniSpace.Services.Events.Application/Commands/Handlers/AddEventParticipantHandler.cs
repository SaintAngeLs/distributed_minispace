using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class AddEventParticipantHandler : ICommandHandler<AddEventParticipant>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public AddEventParticipantHandler(IEventRepository eventRepository, IStudentsServiceClient studentsServiceClient,
            IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(AddEventParticipant command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var studentExists = await _studentsServiceClient.StudentExistsAsync(command.StudentId);
            if (!studentExists)
            {
                throw new StudentNotFoundException(command.StudentId);
            }
            
            var identity = _appContext.Identity;
            var organizerIdMatches = @event.Organizer.OrganizerType == OrganizerType.User 
                ? @event.Organizer.UserId == identity.Id 
                : @event.Organizer.OrganizationId == identity.Id;

            if (identity.IsAuthenticated && !organizerIdMatches && !identity.IsAdmin)
            {
                throw new UnauthorizedEventAccessException(@event.Id, identity.Id);
            }
            
            @event.AddParticipant(new Participant(command.StudentId, command.StudentName));
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new EventParticipantAdded(@event.Id, 
                command.StudentId, command.StudentName));
        }
    }
}
