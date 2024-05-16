using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestCreatedHandler : IEventHandler<FriendRequestCreated>
    {
        private readonly IFriendEventRepository _friendEventRepository;

        public FriendRequestCreatedHandler(IFriendEventRepository friendEventRepository)
        {
            _friendEventRepository = friendEventRepository;
        }

        public async Task HandleAsync(FriendRequestCreated friendEvent, CancellationToken cancellationToken)
        {
            var newFriendEvent = new FriendEvent(
                id: Guid.NewGuid(),
                eventId: Guid.NewGuid(),
                userId: friendEvent.RequesterId, 
                friendId: friendEvent.FriendId,
                eventType: "FriendRequestCreated",
                details: $"A new friend request created from {friendEvent.RequesterId} to {friendEvent.FriendId}",
                createdAt: DateTime.UtcNow
            );

            await _friendEventRepository.AddAsync(newFriendEvent);
        }
    }
}
