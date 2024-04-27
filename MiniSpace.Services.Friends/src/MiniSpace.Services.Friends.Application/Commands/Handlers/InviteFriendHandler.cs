using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class InviteFriendHandler : ICommandHandler<InviteFriend>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;

        public InviteFriendHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext;
        }

        public async Task HandleAsync(InviteFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || identity.Id == command.InviterId)
            {
                // TODO:: UPDATE THE LOGIC HERE FOR THE PRODUCTION.❗
                // throw new UnauthorizedFriendActionException(command.InviterId, command.InviteeId);
            }

            var alreadyFriendsOrInvited = await _friendRepository.IsFriendAsync(command.InviterId, command.InviteeId);
            if (alreadyFriendsOrInvited)
            {
                throw new AlreadyFriendsException(command.InviterId, command.InviteeId);
            }

            await _friendRepository.InviteFriendAsync(command.InviterId, command.InviteeId);
            var friendInvitedEvent = new FriendInvited(command.InviterId, command.InviteeId);

            await _messageBroker.PublishAsync(friendInvitedEvent);
        }

    }
}
