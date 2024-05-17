using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Events;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestCreatedHandler : IEventHandler<FriendRequestCreated>
    {
        private readonly IFriendEventRepository _friendEventRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<FriendRequestCreatedHandler> _logger;

        public FriendRequestCreatedHandler(IFriendEventRepository friendEventRepository, IEventMapper eventMapper, IMessageBroker messageBroker, ILogger<FriendRequestCreatedHandler> logger)
        {
            _friendEventRepository = friendEventRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
             _logger = logger;
        }

        public async Task HandleAsync(FriendRequestCreated friendEvent, CancellationToken cancellationToken)
        {
             _logger.LogInformation($"Received FriendRequestCreated event: RequesterId={friendEvent.RequesterId}, FriendId={friendEvent.FriendId}");
             Console.WriteLine("**************************************************************************************************************");
            var newFriendEvent = new FriendEvent(
                id: Guid.NewGuid(),
                eventId: Guid.NewGuid(),
                userId: friendEvent.RequesterId, 
                friendId: friendEvent.FriendId,
                eventType: "FriendRequestCreated",
                details: $"A new friend request created from {friendEvent.RequesterId} to {friendEvent.FriendId}",
                createdAt: DateTime.UtcNow
            );

             var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: friendEvent.RequesterId, 
                message: $"You have received a friend request from userId: {friendEvent.RequesterId}",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            await _friendEventRepository.AddAsync(newFriendEvent);
            
            await _messageBroker.PublishAsync(friendEvent);

            await _notificationRepository.AddAsync(notification);
            _logger.LogInformation($"Stored new friend event and notification for UserId={friendEvent.RequesterId}");
            var notificationCreated = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt
            );

            await _messageBroker.PublishAsync(notificationCreated);
              _logger.LogInformation($"Published NotificationCreated event for NotificationId={notification.NotificationId}");
        }
    }
}
