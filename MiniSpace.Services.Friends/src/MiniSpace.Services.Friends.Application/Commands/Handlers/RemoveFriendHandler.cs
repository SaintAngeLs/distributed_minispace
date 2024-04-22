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
        private readonly IAppContext _appContext; 

        public RemoveFriendHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext; 
        }

        public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || (identity.Id != command.RequesterId && !identity.IsAdmin))
            {
                throw new UnauthorizedAccessException("Not authorized to remove friend.");
            }

            // Validate the friendship exists
            var exists = await _friendRepository.IsFriendAsync(command.RequesterId, command.FriendId);
            if (!exists)
            {
                throw new InvalidOperationException("No such friendship exists.");
            }

            await _friendRepository.RemoveFriendAsync(command.RequesterId, command.FriendId);
            var eventToPublish = _eventMapper.Map(new PendingFriendDeclined(command.RequesterId, command.FriendId));
            await _messageBroker.PublishAsync(eventToPublish);
        }
    }
}
