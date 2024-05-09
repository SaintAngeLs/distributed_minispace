using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class SignUpToEventHandler : ICommandHandler<SignUpToEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public SignUpToEventHandler(IEventRepository eventRepository, IStudentRepository studentRepository, 
            IMessageBroker messageBroker, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _studentRepository = studentRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(SignUpToEvent command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.StudentId)
            {
                throw new UnauthorizedEventAccessException(command.EventId, command.StudentId);
            }
            
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }

            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var participant = new Participant(student.Id, identity.Name);
            @event.SignUpStudent(participant);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new StudentSignedUpToEvent(@event.Id, student.Id));
        }
    }
}