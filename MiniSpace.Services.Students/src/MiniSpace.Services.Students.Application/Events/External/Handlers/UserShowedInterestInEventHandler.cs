using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserShowedInterestInEventHandler : IEventHandler<StudentShowedInterestInEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UserShowedInterestInEventHandler(IUserRepository userRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(StudentShowedInterestInEvent @event, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetAsync(@event.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(@event.StudentId);
            }
            
            student.AddInterestedInEvent(@event.EventId);
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
