using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserCancelledInterestInEventHandler : IEventHandler<UserCancelledInterestInEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UserCancelledInterestInEventHandler(IUserRepository userRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(UserCancelledInterestInEvent @event, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetAsync(@event.StudentId);
            if (student is null)
            {
                throw new UserNotFoundException(@event.StudentId);
            }
            
            student.RemoveInterestedInEvent(@event.EventId);
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}