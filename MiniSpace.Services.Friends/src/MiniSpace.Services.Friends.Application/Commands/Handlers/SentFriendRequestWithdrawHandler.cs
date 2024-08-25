using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class SentFriendRequestWithdrawHandler : ICommandHandler<SentFriendRequestWithdraw>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRequestsRepository _userRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;
        private readonly ILogger<SentFriendRequestWithdrawHandler> _logger;

        public SentFriendRequestWithdrawHandler(
            IFriendRequestRepository friendRequestRepository,
            IUserRequestsRepository userRequestsRepository,
            IMessageBroker messageBroker,
            IAppContext appContext,
            ILogger<SentFriendRequestWithdrawHandler> logger)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRequestsRepository = userRequestsRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
            _logger = logger;
        }

        public async Task HandleAsync(SentFriendRequestWithdraw command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling SentFriendRequestWithdraw command: InviterId: {InviterId}, InviteeId: {InviteeId}", command.InviterId, command.InviteeId);

            var inviterRequests = await _userRequestsRepository.GetAsync(command.InviterId);
            var inviteeRequests = await _userRequestsRepository.GetAsync(command.InviteeId);

            var friendRequestForInviter = inviterRequests?.FriendRequests.FirstOrDefault(fr => fr.InviterId == command.InviterId && fr.InviteeId == command.InviteeId);
            var friendRequestForInvitee = inviteeRequests?.FriendRequests.FirstOrDefault(fr => fr.InviteeId == command.InviteeId && fr.InviterId == command.InviterId);

            if (friendRequestForInviter == null || friendRequestForInvitee == null)
            {
                _logger.LogError("Friend request not found for InviterId: {InviterId} and InviteeId: {InviteeId}", command.InviterId, command.InviteeId);
                throw new FriendRequestNotFoundException(command.InviterId, command.InviteeId);
            }

            friendRequestForInviter.State = FriendState.Cancelled;
            friendRequestForInvitee.State = FriendState.Cancelled;
            _logger.LogInformation("Updating friend request state to Cancelled for request ID: {RequestId}", friendRequestForInviter.Id);

            await UpdateAndSaveRequests(inviterRequests, friendRequestForInviter);
            await UpdateAndSaveRequests(inviteeRequests, friendRequestForInvitee);

            await _friendRequestRepository.DeleteAsync(friendRequestForInviter.Id);

            await _messageBroker.PublishAsync(new FriendRequestWithdrawn(friendRequestForInviter.InviterId, friendRequestForInvitee.InviteeId));
            _logger.LogInformation("Published FriendRequestWithdrawn event for InviterId: {InviterId} and InviteeId: {InviteeId}", 
            friendRequestForInviter.InviterId, friendRequestForInvitee.InviteeId);
        }

        private async Task UpdateAndSaveRequests(UserRequests requests, FriendRequest friendRequest)
        {
            if (requests == null)
            {
                _logger.LogWarning("Received null UserRequests object for FriendRequest ID: {FriendRequestId}", friendRequest.Id);
                return;
            }

            if (!requests.FriendRequests.Any(fr => fr.Id == friendRequest.Id))
            {
                _logger.LogWarning("FriendRequest ID: {FriendRequestId} not found in the requests of User ID: {UserId}", friendRequest.Id, requests.UserId);
                return;
            }

            requests.RemoveRequest(friendRequest.Id);

            await _userRequestsRepository.UpdateAsync(requests.UserId, requests.FriendRequests.ToList());
            _logger.LogInformation("Updated and saved requests successfully for UserId: {UserId}, Total Requests: {Count}", requests.UserId, requests.FriendRequests.Count());
        }
    }
}
