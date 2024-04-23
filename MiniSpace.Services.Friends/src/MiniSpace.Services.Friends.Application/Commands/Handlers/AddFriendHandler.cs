using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Application.Services;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class AddFriendHandler : ICommandHandler<AddFriend>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddFriendHandler(IFriendRepository friendRepository, IAppContext appContext,
                                IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _friendRepository = friendRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(AddFriend command, CancellationToken cancellationToken = default)
        {
            ValidateAccessOrFail(command.RequesterId);

            var alreadyFriends = await _friendRepository.IsFriendAsync(command.RequesterId, command.FriendId);
            if (alreadyFriends)
            {
                throw new AlreadyFriendsException();
            }

            var requester = await _friendRepository.GetFriendshipAsync(command.RequesterId, command.FriendId);
            if (requester == null)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }
            await _friendRepository.UpdateFriendshipAsync(requester);
            var events = _eventMapper.MapAll(requester.Events);
            await _messageBroker.PublishAsync(events);
        }

        private void ValidateAccessOrFail(Guid requesterId)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || identity.Id != requesterId)
            {
                throw new UnauthorizedFriendActionException();
            }
        }
    }
}
