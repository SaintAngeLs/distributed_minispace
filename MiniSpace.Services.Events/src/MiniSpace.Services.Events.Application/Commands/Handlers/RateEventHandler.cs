using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class RateEventHandler : ICommandHandler<RateEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentRepository _studentRepository;

        public RateEventHandler(IEventRepository eventRepository, IStudentRepository studentRepository)
        {
            _eventRepository = eventRepository;
            _studentRepository = studentRepository;
        }

        public async Task HandleAsync(RateEvent command, CancellationToken cancellationToken)
        {
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

            @event.Rate(command.StudentId, command.Rating);
            await _eventRepository.UpdateAsync(@event);
        }
    }
}