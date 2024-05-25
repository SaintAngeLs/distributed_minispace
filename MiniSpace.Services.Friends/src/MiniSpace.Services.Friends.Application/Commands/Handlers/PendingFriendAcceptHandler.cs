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
        private readonly IStudentRequestsRepository _studentRequestsRepository;
        private readonly IStudentFriendsRepository _studentFriendsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

         public PendingFriendAcceptHandler(
            // IFriendRequestRepository friendRequestRepository,
            IStudentFriendsRepository studentFriendsRepository,
            IStudentRequestsRepository studentRequestsRepository,
            // IFriendRepository friendRepository,
            IMessageBroker messageBroker,
            IEventMapper eventMapper)
            
        {
            // _friendRequestRepository = friendRequestRepository;
            _studentFriendsRepository = studentFriendsRepository;
            _studentRequestsRepository = studentRequestsRepository;
            // _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(PendingFriendAccept command, CancellationToken cancellationToken = default)
        {
            // Retrieve and validate the friend request between the inviter and invitee
            var inviterRequests = await _studentRequestsRepository.GetAsync(command.RequesterId);
            var inviteeRequests = await _studentRequestsRepository.GetAsync(command.FriendId);
            var friendRequest = FindFriendRequest(inviterRequests, inviteeRequests, command.RequesterId, command.FriendId);
            var friendRequestInvitee = FindFriendRequest(inviteeRequests, inviterRequests, command.RequesterId, command.FriendId);
            // Update the friend request state to accepted
            friendRequest.State = FriendState.Accepted;
            friendRequestInvitee.State = FriendState.Accepted;

            // Save the updated FriendRequest states
            await _studentRequestsRepository.UpdateAsync(command.RequesterId, inviterRequests.FriendRequests);
            await _studentRequestsRepository.UpdateAsync(command.FriendId, inviteeRequests.FriendRequests);

            // Create Friend relationships in both directions
            CreateAndAddFriends(command.RequesterId, command.FriendId, FriendState.Accepted);

            // Publish related events
            // var events = _eventMapper.MapAll(new Core.Events.PendingFriendAccepted(command.RequesterId, command.FriendId));
            // await _messageBroker.PublishAsync(events);

            var pendingFriendAcceptedEvent = new PendingFriendAccepted(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(pendingFriendAcceptedEvent);
        }

        private FriendRequest FindFriendRequest(StudentRequests inviter, StudentRequests invitee, Guid inviterId, Guid inviteeId)
        {
            // Find the FriendRequest in both inviter and invitee collections
            return inviter.FriendRequests.FirstOrDefault(fr => fr.InviterId == inviterId && fr.InviteeId == inviteeId)
                ?? invitee.FriendRequests.FirstOrDefault(fr => fr.InviterId == inviterId && fr.InviteeId == inviteeId)
                ?? throw new FriendRequestNotFoundException(inviterId, inviteeId);
        }

        private async void CreateAndAddFriends(Guid inviterId, Guid inviteeId, FriendState state)
        {
            // Retrieve or initialize the StudentFriends for both inviter and invitee
            var inviterFriends = await _studentFriendsRepository.GetAsync(inviterId) ?? new StudentFriends(inviterId);
            var inviteeFriends = await _studentFriendsRepository.GetAsync(inviteeId) ?? new StudentFriends(inviteeId);

            // Add new Friend instances with accepted state
            inviterFriends.AddFriend(new Friend(inviterId, inviteeId, DateTime.UtcNow, state));
            inviteeFriends.AddFriend(new Friend(inviteeId, inviterId, DateTime.UtcNow, state));

            // Update the StudentFriends repositories
            await _studentFriendsRepository.AddOrUpdateAsync(inviterFriends);
            await _studentFriendsRepository.AddOrUpdateAsync(inviteeFriends);
        }
    }
}
