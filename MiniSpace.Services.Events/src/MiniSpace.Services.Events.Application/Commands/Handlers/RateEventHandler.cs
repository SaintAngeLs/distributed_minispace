using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class RateEventHandler : ICommandHandler<RateEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public RateEventHandler(IEventRepository eventRepository, IStudentsServiceClient studentsServiceClient)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(RateEvent command, CancellationToken cancellationToken)
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

            @event.Rate(command.StudentId, command.Rating);
            await _eventRepository.UpdateAsync(@event);
        }
    }
}
