using Paralax.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using System.Text.Json;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class InviteFriendHandler : ICommandHandler<InviteFriend>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRequestsRepository _userRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;

        public InviteFriendHandler(
            IFriendRequestRepository friendRequestRepository,
            IUserRequestsRepository userRequestsRepository, 
            IMessageBroker messageBroker, 
            IEventMapper eventMapper, 
            IAppContext appContext)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRequestsRepository = userRequestsRepository;
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

            var existingRequest = await _friendRequestRepository.FindByInviterAndInvitee(command.InviterId, command.InviteeId);

            if (existingRequest != null && existingRequest.State == FriendState.Requested)
            {
                throw new AlreadyInvitedException(command.InviterId, command.InviteeId);
            }

            var friendRequest = new FriendRequest(
                inviterId: command.InviterId,
                inviteeId: command.InviteeId,
                requestedAt: DateTime.UtcNow,
                state: FriendState.Requested
            );

            await AddOrUpdateUserRequest(command.InviterId, friendRequest, FriendState.Requested);
            await AddOrUpdateUserRequest(command.InviteeId, friendRequest, FriendState.Pending);

            // Publish FriendInvited Event
            var friendInvitedEvent = new FriendInvited(command.InviterId, command.InviteeId);
            string friendInvitedJson = JsonSerializer.Serialize(friendInvitedEvent);
            await _messageBroker.PublishAsync(friendInvitedEvent);
        }

        private async Task AddOrUpdateUserRequest(Guid userId, FriendRequest friendRequest, FriendState state)
        {
            var userRequests = await _userRequestsRepository.GetAsync(userId);
            if (userRequests == null)
            {
                userRequests = new UserRequests(userId);
                await _userRequestsRepository.AddAsync(userRequests);
            }

            userRequests.AddRequest(friendRequest.InviterId, friendRequest.InviteeId, friendRequest.RequestedAt, state);
            await _userRequestsRepository.UpdateAsync(userRequests.UserId, userRequests.FriendRequests);
        }

    }
}
