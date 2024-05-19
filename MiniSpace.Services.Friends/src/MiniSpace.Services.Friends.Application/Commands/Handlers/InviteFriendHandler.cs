using Convey.CQRS.Commands;
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
        private readonly IStudentRequestsRepository _studentRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;

        public InviteFriendHandler(
            IFriendRequestRepository friendRequestRepository,
            IStudentRequestsRepository studentRequestsRepository, 
            IMessageBroker messageBroker, 
            IEventMapper eventMapper, 
            IAppContext appContext)
        {
            _friendRequestRepository = friendRequestRepository;
            _studentRequestsRepository = studentRequestsRepository;
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

            await _friendRequestRepository.AddAsync(friendRequest);

            await AddOrUpdateStudentRequest(command.InviterId, friendRequest, FriendState.Requested);
            await AddOrUpdateStudentRequest(command.InviteeId, friendRequest, FriendState.Pending);


    //         // Optionally, publish an event about the friend request
    //         var friendInvitedEvent = new FriendInvited(command.InviterId, command.InviteeId);

    //           string eventJson = JsonSerializer.Serialize(friendInvitedEvent);
    // Console.WriteLine($"Publishing event: {eventJson}");
    //         await _messageBroker.PublishAsync(friendInvitedEvent);


            // Publish FriendInvited Event
            var friendInvitedEvent = new FriendInvited(command.InviterId, command.InviteeId);
            string friendInvitedJson = JsonSerializer.Serialize(friendInvitedEvent);
            // Console.WriteLine($"Publishing FriendInvited event: {friendInvitedJson}");
            await _messageBroker.PublishAsync(friendInvitedEvent);

            // Publish FriendRequestCreated Event
            var friendRequestCreatedEvent = new FriendRequestCreated(command.InviterId, command.InviteeId);
            string friendRequestCreatedJson = JsonSerializer.Serialize(friendRequestCreatedEvent);
            // Console.WriteLine($"Publishing FriendRequestCreated event: {friendRequestCreatedJson}");
            await _messageBroker.PublishAsync(friendRequestCreatedEvent);

            // Publish FriendRequestSent Event
            var friendRequestSentEvent = new FriendRequestSent(command.InviterId, command.InviteeId);
            string friendRequestSentJson = JsonSerializer.Serialize(friendRequestSentEvent);
            // Console.WriteLine($"Publishing FriendRequestSent event: {friendRequestSentJson}");
            await _messageBroker.PublishAsync(friendRequestSentEvent);

            
        }

         private async Task AddOrUpdateStudentRequest(Guid studentId, FriendRequest friendRequest, FriendState state)
        {
            var studentRequests = await _studentRequestsRepository.GetAsync(studentId);
            if (studentRequests == null)
            {
                studentRequests = new StudentRequests(studentId);
                await _studentRequestsRepository.AddAsync(studentRequests);
            }

            studentRequests.AddRequest(friendRequest.InviterId, friendRequest.InviteeId, friendRequest.RequestedAt, state);
            await _studentRequestsRepository.UpdateAsync(studentRequests);
        }

    }
}
