using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserBannedHandler : IEventHandler<UserBanned>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UserBannedHandler(IUserRepository userRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(UserBanned @event, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                throw new UserNotFoundException(@event.UserId);
            }
            
            student.Ban();
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
