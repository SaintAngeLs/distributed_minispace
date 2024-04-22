using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class AddFriendHandler : ICommandHandler<AddFriend>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddFriendHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(AddFriend command, CancellationToken cancellationToken = default)
        {
            var alreadyFriends = await _friendRepository.IsFriendAsync(command.RequesterId, command.FriendId);
            if (alreadyFriends)
            {
                throw new InvalidOperationException("You are already friends.");
            }
            
            await _friendRepository.AddFriendAsync(command.RequesterId, command.FriendId);
            var events = new List<IDomainEvent> { new FriendRequestCreated(command.RequesterId, command.FriendId) };
            var eventToPublish = _eventMapper.MapAll(events);
            await _messageBroker.PublishAsync(eventToPublish);
        }
    }
}
