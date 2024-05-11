using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using System.Text.Json;
using System;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
{
    public class PendingFriendDeclinedHandler : IEventHandler<PendingFriendDeclined>
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public PendingFriendDeclinedHandler(
            IFriendRequestRepository friendRequestRepository, 
            IEventMapper eventMapper, 
            IMessageBroker messageBroker, 
            IAppContext appContext)
        {
            _friendRequestRepository = friendRequestRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(PendingFriendDeclined @event, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Handling event: {JsonSerializer.Serialize(@event)}");

            Console.WriteLine($"Searching for friend request between {@event.RequesterId} and {@event.FriendId}");
            var friendRequest = await _friendRequestRepository.FindByInviterAndInvitee(@event.RequesterId, @event.FriendId);

            if (friendRequest == null)
            {
                Console.WriteLine("No friend request found, throwing exception.");
                throw new FriendshipNotFoundException(@event.RequesterId, @event.FriendId);
            }

            if (friendRequest.State != FriendState.Declined)
            {
                Console.WriteLine("Friend request found but not declined, declining now.");
                friendRequest.Decline();
                friendRequest.State = FriendState.Declined;
                await _friendRequestRepository.UpdateAsync(friendRequest);
            }

            Console.WriteLine("Publishing events related to the decline.");
            // var events = _eventMapper.MapAll(friendRequest.Events);
            // await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
