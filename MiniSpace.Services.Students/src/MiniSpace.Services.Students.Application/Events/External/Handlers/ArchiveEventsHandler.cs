using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Application.Events.External;
using Paralax.MessageBrokers;
using MiniSpace.Services.Students.Application.Services;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class EventArchivedHandler : IEventHandler<EventArchived>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMessageBroker _messageBroker;

        public EventArchivedHandler(IStudentRepository studentRepository, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(EventArchived @event, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetStudentsByEventIdAsync(@event.EventId);

            if (students is null || students.Count == 0)
            {
                return;
            }

            foreach (var student in students)
            {
                student.RemoveEvent(@event.EventId);

                await _studentRepository.UpdateAsync(student);
            }

        }
    }
}
