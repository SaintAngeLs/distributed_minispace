using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class AddEventParticipantHandler: ICommandHandler<AddEventParticipant>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private IMessageBroker _messageBroker;
        
        public AddEventParticipantHandler(IEventRepository eventRepository, IStudentRepository studentRepository,
            IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _studentRepository = studentRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(AddEventParticipant command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if(@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var student = await _studentRepository.GetAsync(command.StudentId);
            if(student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }
            
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && @event.OrganizerId != identity.Id && !identity.IsAdmin)
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