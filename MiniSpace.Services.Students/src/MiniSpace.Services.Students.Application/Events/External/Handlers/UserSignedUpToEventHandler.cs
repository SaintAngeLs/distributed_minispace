using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserSignedUpToEventHandler : IEventHandler<StudentSignedUpToEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UserSignedUpToEventHandler(IUserRepository userRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(StudentSignedUpToEvent studentSignedUpToEvent, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetAsync(studentSignedUpToEvent.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(studentSignedUpToEvent.StudentId);
            }
            
            student.AddSignedUpEvent(studentSignedUpToEvent.EventId);
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
