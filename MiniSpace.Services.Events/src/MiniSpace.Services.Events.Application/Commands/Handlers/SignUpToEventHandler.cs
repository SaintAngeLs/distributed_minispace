using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class SignUpToEventHandler : ICommandHandler<SignUpToEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMessageBroker _messageBroker;

        public SignUpToEventHandler(IEventRepository eventRepository, IStudentRepository studentRepository, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _studentRepository = studentRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(SignUpToEvent command)
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

            if (@event.RegisteredStudents.Any(s => s.Id == student.Id))
            {
                return;
            }

            @event.RegisterStudent(student);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new StudentSignedUpToEvent(@event.Id, student.Id));
        }
    }
}