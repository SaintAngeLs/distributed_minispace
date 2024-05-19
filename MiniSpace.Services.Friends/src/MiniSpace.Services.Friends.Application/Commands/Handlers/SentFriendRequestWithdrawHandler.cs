using Convey.CQRS.Commands;
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

        public SentFriendRequestWithdrawHandler(
            IFriendRequestRepository friendRequestRepository,
            IStudentRequestsRepository studentRequestsRepository,
            IMessageBroker messageBroker,
            IAppContext appContext)
        {
            _friendRequestRepository = friendRequestRepository;
            _studentRequestsRepository = studentRequestsRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(SentFriendRequestWithdraw command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || identity.Id != command.InviterId)
            {
                throw new UnauthorizedFriendActionException(command.InviterId, command.InviteeId);
            }

            var friendRequest = await _friendRequestRepository.FindByInviterAndInvitee(command.InviterId, command.InviteeId);

            if (friendRequest == null || friendRequest.State != FriendState.Requested)
            {
                throw new FriendRequestNotFoundException(command.InviterId, command.InviteeId);
            }

            friendRequest.Cancel();

            await _friendRequestRepository.UpdateAsync(friendRequest);

            var inviterRequests = await _studentRequestsRepository.GetAsync(command.InviterId);
            inviterRequests?.RemoveRequest(friendRequest.Id);
            await _studentRequestsRepository.UpdateAsync(inviterRequests);

            var inviteeRequests = await _studentRequestsRepository.GetAsync(command.InviteeId);
            inviteeRequests?.RemoveRequest(friendRequest.Id);
            await _studentRequestsRepository.UpdateAsync(inviteeRequests);

            await _messageBroker.PublishAsync(new FriendRequestWithdrawn(command.InviterId, command.InviteeId));
        }
    }
}
