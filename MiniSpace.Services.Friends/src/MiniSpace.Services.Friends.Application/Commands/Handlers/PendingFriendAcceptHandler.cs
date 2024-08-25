using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Application.Events.External;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class PendingFriendAcceptHandler : ICommandHandler<PendingFriendAccept>
    {
        private readonly IUserRequestsRepository _userRequestsRepository;
        private readonly IUserFriendsRepository _userFriendsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

         public PendingFriendAcceptHandler(
            IUserRequestsRepository userFriendsRepository,
            IUserFriendsRepository userRequestsRepository,
            IMessageBroker messageBroker,
            IEventMapper eventMapper)
            
        {
            _userRequestsRepository = userFriendsRepository;
            _userFriendsRepository = userRequestsRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(PendingFriendAccept command, CancellationToken cancellationToken = default)
        {
            var inviterRequests = await _userRequestsRepository.GetAsync(command.RequesterId);
            var inviteeRequests = await _userRequestsRepository.GetAsync(command.FriendId);
            var friendRequest = FindFriendRequest(inviterRequests, inviteeRequests, command.RequesterId, command.FriendId);
            var friendRequestInvitee = FindFriendRequest(inviteeRequests, inviterRequests, command.RequesterId, command.FriendId);

            friendRequest.State = FriendState.Accepted;
            friendRequestInvitee.State = FriendState.Accepted;

            await _userRequestsRepository.UpdateAsync(command.RequesterId, inviterRequests.FriendRequests);
            await _userRequestsRepository.UpdateAsync(command.FriendId, inviteeRequests.FriendRequests);
            
            CreateAndAddFriends(command.RequesterId, command.FriendId, FriendState.Accepted); 
            
            var pendingFriendAcceptedEvent = new PendingFriendAccepted(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(pendingFriendAcceptedEvent);
        }

        private FriendRequest FindFriendRequest(UserRequests inviter, UserRequests invitee, Guid inviterId, Guid inviteeId)
        {
            return inviter.FriendRequests.FirstOrDefault(fr => fr.InviterId == inviterId && fr.InviteeId == inviteeId)
                ?? invitee.FriendRequests.FirstOrDefault(fr => fr.InviterId == inviterId && fr.InviteeId == inviteeId)
                ?? throw new FriendRequestNotFoundException(inviterId, inviteeId);
        }

        private async void CreateAndAddFriends(Guid inviterId, Guid inviteeId, FriendState state)
        {
            var inviterFriends = await _userFriendsRepository.GetAsync(inviterId) ?? new UserFriends(inviterId);
            var inviteeFriends = await _userFriendsRepository.GetAsync(inviteeId) ?? new UserFriends(inviteeId);

            inviterFriends.AddFriend(new Friend(inviterId, inviteeId, DateTime.UtcNow, state));
            inviteeFriends.AddFriend(new Friend(inviteeId, inviterId, DateTime.UtcNow, state));

            await _userFriendsRepository.AddOrUpdateAsync(inviterFriends);
            await _userFriendsRepository.AddOrUpdateAsync(inviteeFriends);
        }
    }
}
