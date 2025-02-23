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
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public EventArchivedHandler(IUserRepository userRepository, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(EventArchived @event, CancellationToken cancellationToken)
        {
            var students = await _userRepository.GetUsersByEventIdAsync(@event.EventId);

            if (students is null || students.Count == 0)
            {
                return;
            }

            foreach (var student in students)
            {
                student.RemoveEvent(@event.EventId);

                await _userRepository.UpdateAsync(student);
            }

        }
    }
}
