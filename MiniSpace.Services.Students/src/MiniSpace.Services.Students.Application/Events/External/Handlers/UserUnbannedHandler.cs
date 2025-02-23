using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserUnbannedHandler : IEventHandler<UserUnbanned>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UserUnbannedHandler(IUserRepository userRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(UserUnbanned @event, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                throw new UserNotFoundException(@event.UserId);
            }
            
            student.Unban();
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
