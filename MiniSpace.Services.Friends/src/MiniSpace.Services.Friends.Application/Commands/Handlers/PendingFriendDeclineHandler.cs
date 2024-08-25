using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using System;
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

            if (friendRequest.State != Core.Entities.FriendState.Requested)
            {
                throw new InvalidOperationException("Friend request is not in the correct state to be declined.");
            }

            friendRequest.Decline();
            friendRequest.State = Core.Entities.FriendState.Declined;
            await _friendRequestRepository.UpdateAsync(friendRequest);
        }
    }
}
