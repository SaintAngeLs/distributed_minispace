using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class CancelSignUpToEventHandler : ICommandHandler<CancelSignUpToEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public CancelSignUpToEventHandler(IEventRepository eventRepository, IMessageBroker messageBroker, 
            IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CancelSignUpToEvent command, CancellationToken cancellationToken)
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

            @event.CancelSignUp(command.StudentId);
            await _eventRepository.UpdateAsync(@event);
        }
    }
}