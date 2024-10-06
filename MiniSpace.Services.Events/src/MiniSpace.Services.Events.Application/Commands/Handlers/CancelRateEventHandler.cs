using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class CancelRateEventHandler : ICommandHandler<CancelRateEvent>
    {
        private readonly IEventRepository _eventRepository;

        public CancelRateEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task HandleAsync(CancelRateEvent command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }

            @event.CancelRate(command.StudentId);
            await _eventRepository.UpdateAsync(@event);
        }
    }
}