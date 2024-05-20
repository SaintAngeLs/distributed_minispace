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
            // Retrieve the StudentRequests entities for both inviter and invitee
            var inviterRequests = await _studentRequestsRepository.GetAsync(command.RequesterId);
            var inviteeRequests = await _studentRequestsRepository.GetAsync(command.FriendId);

            // Retrieve the specific FriendRequest
            var friendRequest = inviterRequests?.FriendRequests.FirstOrDefault(fr => fr.InviterId == command.RequesterId && fr.InviteeId == command.FriendId)
                                ?? inviteeRequests?.FriendRequests.FirstOrDefault(fr => fr.InviterId == command.RequesterId && fr.InviteeId == command.FriendId);

            if (friendRequest == null)
            {
                throw new FriendRequestNotFoundException(command.RequesterId, command.FriendId);
            }

            // if (friendRequest.State != FriendState.Pending)
            // {
            //     throw new InvalidFriendRequestStateException(friendRequest.State);
            // }

            // Create new Friend entities for both students
            var friendForInviter = new Friend(friendRequest.InviterId, friendRequest.InviteeId, DateTime.UtcNow, FriendState.Accepted);
            var friendForInvitee = new Friend(friendRequest.InviteeId, friendRequest.InviterId, DateTime.UtcNow, FriendState.Accepted);

            // Retrieve the StudentFriends entities
            var inviterFriends = await _studentFriendsRepository.GetAsync(friendRequest.InviterId) ?? new StudentFriends(friendRequest.InviterId);
            var inviteeFriends = await _studentFriendsRepository.GetAsync(friendRequest.InviteeId) ?? new StudentFriends(friendRequest.InviteeId);

            // Add the new friends
            inviterFriends.AddFriend(friendForInviter);
            inviteeFriends.AddFriend(friendForInvitee);

            // Save the updated StudentFriends entities
            await _studentFriendsRepository.AddOrUpdateAsync(inviterFriends);
            await _studentFriendsRepository.AddOrUpdateAsync(inviteeFriends);

            // Update the friend request state to accepted and save
            friendRequest.Accept();
            await UpdateAndSaveRequests(inviterRequests, friendRequest);
            await UpdateAndSaveRequests(inviteeRequests, friendRequest);

            // Publish events
            var events = _eventMapper.MapAll(new Core.Events.PendingFriendAccepted(friendRequest.InviterId, friendRequest.InviteeId));
            await _messageBroker.PublishAsync(events);
        }

        private async Task UpdateAndSaveRequests(StudentRequests requests, FriendRequest friendRequest)
        {
            if (requests != null && requests.FriendRequests.Any(fr => fr.Id == friendRequest.Id))
            {
                requests.UpdateRequestState(friendRequest.Id, FriendState.Accepted);

                // Save the updated StudentRequests entity
                await _studentRequestsRepository.UpdateAsync(requests);
            }
        }
    }
}
