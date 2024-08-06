using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class SignUpToEventHandler : ICommandHandler<SignUpToEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public SignUpToEventHandler(IEventRepository eventRepository, IStudentsServiceClient studentsServiceClient, 
            IMessageBroker messageBroker, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
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

            var studentExists = await _studentsServiceClient.StudentExistsAsync(command.StudentId);
            if (!studentExists)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var participant = new Participant(command.StudentId, identity.Name);
            @event.SignUpParticipant(participant);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new StudentSignedUpToEvent(@event.Id, command.StudentId));
        }
    }
}
