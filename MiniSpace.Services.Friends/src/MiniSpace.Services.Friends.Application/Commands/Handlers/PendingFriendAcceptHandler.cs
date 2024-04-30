using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class PendingFriendAcceptHandler : ICommandHandler<PendingFriendAccept>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

         public PendingFriendAcceptHandler(
            IFriendRequestRepository friendRequestRepository,
            IFriendRepository friendRepository,
            IMessageBroker messageBroker,
            IEventMapper eventMapper)
            
        {
            _friendRequestRepository = friendRequestRepository;
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(PendingFriendAccept command, CancellationToken cancellationToken = default)
        {
            var friendRequest = await _friendRequestRepository.FindByInviterAndInvitee(command.RequesterId, command.FriendId);
            if (friendRequest == null)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

            if (friendRequest.State != FriendState.Requested)
            {
                throw new InvalidOperationException("Friend request is not in the correct state to be accepted.");
            }

            friendRequest.Accept();
            await _friendRequestRepository.UpdateAsync(friendRequest);

            var newFriend = new Friend(command.RequesterId, command.FriendId, DateTime.UtcNow, FriendState.Accepted);
            await _friendRepository.AddAsync(newFriend);
        }
    }
}
