using Paralax.CQRS.Commands;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Application.Events.External;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class PendingFriendDeclineHandler : ICommandHandler<PendingFriendDecline>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public PendingFriendDeclineHandler(
            IFriendRequestRepository friendRequestRepository,
            IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _friendRequestRepository = friendRequestRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(PendingFriendDecline command, CancellationToken cancellationToken = default)
        {
            var friendRequest = await _friendRequestRepository.FindByInviterAndInvitee(command.RequesterId, command.FriendId);
            if (friendRequest == null)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

             if (friendRequest.State != FriendState.Requested)
            {
                throw new InvalidFriendRequestStateException(command.RequesterId, command.FriendId, friendRequest.State.ToString());
            }

            friendRequest.Decline();
            friendRequest.State = FriendState.Declined;
            await _friendRequestRepository.UpdateAsync(friendRequest);

            var pendingFriendDeclinedEvent = new PendingFriendDeclined(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(pendingFriendDeclinedEvent);
        }
    }
}
