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
        private readonly IStudentRequestsRepository _studentRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;
        private readonly ILogger<SentFriendRequestWithdrawHandler> _logger;

        public SentFriendRequestWithdrawHandler(
            IFriendRequestRepository friendRequestRepository,
            IStudentRequestsRepository studentRequestsRepository,
            IMessageBroker messageBroker,
            IAppContext appContext,
            ILogger<SentFriendRequestWithdrawHandler> logger)
        {
            _friendRequestRepository = friendRequestRepository;
            _studentRequestsRepository = studentRequestsRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
            _logger = logger;
        }

       public async Task HandleAsync(SentFriendRequestWithdraw command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling SentFriendRequestWithdraw command: InviterId: {InviterId}, InviteeId: {InviteeId}", command.InviterId, command.InviteeId);

            var inviterRequests = await _studentRequestsRepository.GetAsync(command.InviterId);
            var inviteeRequests = await _studentRequestsRepository.GetAsync(command.InviteeId);

            var friendRequest = inviterRequests?.FriendRequests.FirstOrDefault(fr => fr.InviterId == command.InviterId && fr.InviteeId == command.InviteeId)
                                ?? inviteeRequests?.FriendRequests.FirstOrDefault(fr => fr.InviterId == command.InviterId && fr.InviteeId == command.InviteeId);

            if (friendRequest == null)
            {
                _logger.LogError("Friend request not found for InviterId: {InviterId} and InviteeId: {InviteeId}", command.InviterId, command.InviteeId);
                throw new FriendRequestNotFoundException(command.InviterId, command.InviteeId);
            }

            friendRequest.State = FriendState.Cancelled;
            _logger.LogInformation("Updating friend request state to Cancelled for request ID: {RequestId}", friendRequest.Id);

            // Update the states and remove from both request lists
            await UpdateAndSaveRequests(inviterRequests, friendRequest);
            await UpdateAndSaveRequests(inviteeRequests, friendRequest);

            // Optionally delete the friend request if no longer needed
            await _friendRequestRepository.DeleteAsync(friendRequest.Id);
            _logger.LogInformation("Deleted friend request from the database for request ID: {RequestId}", friendRequest.Id);

            await _messageBroker.PublishAsync(new FriendRequestWithdrawn(friendRequest.InviterId, friendRequest.InviteeId));
            _logger.LogInformation("Published FriendRequestWithdrawn event for InviterId: {InviterId} and InviteeId: {InviteeId}", friendRequest.InviterId, friendRequest.InviteeId);
        }

        private async Task UpdateAndSaveRequests(StudentRequests requests, FriendRequest friendRequest)
        {
            if (requests != null && requests.FriendRequests.Any(fr => fr.Id == friendRequest.Id))
            {
                requests.UpdateRequestState(friendRequest.Id, FriendState.Cancelled);
                requests.RemoveRequest(friendRequest.Id);

                // Ensure the updated list of FriendRequests is not empty
                var updatedFriendRequests = requests.FriendRequests.Any() ? requests.FriendRequests : new List<FriendRequest>();

                // Call the updated UpdateAsync method with two arguments
                await _studentRequestsRepository.UpdateAsync(requests.StudentId, updatedFriendRequests);
                
                _logger.LogInformation("Updated requests successfully in the database for StudentId: {StudentId}", requests.StudentId);
            }
        }


    }
}
