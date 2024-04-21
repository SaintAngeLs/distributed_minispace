using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class RemoveFriendHandler : ICommandHandler<RemoveFriend>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public RemoveFriendHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            await _friendRepository.RemoveFriendAsync(command.RequesterId, command.FriendId);
            var eventToPublish = _eventMapper.Map(new PendingFriendDeclined(command.RequesterId, command.FriendId));
            await _messageBroker.PublishAsync(eventToPublish);
        }
    }
}
