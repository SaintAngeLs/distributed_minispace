using System.Text.Json;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Events;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestCreatedHandler : IEventHandler<FriendRequestCreated>
    {
        private readonly IFriendEventRepository _friendEventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<FriendRequestCreatedHandler> _logger;

        public FriendRequestCreatedHandler(IFriendEventRepository friendEventRepository, IStudentsServiceClient studentsServiceClient, IEventMapper eventMapper, IMessageBroker messageBroker, ILogger<FriendRequestCreatedHandler> logger)
        {
            _friendEventRepository = friendEventRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(FriendRequestCreated friendEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Received FriendRequestCreated event: RequesterId={friendEvent.RequesterId}, FriendId={friendEvent.FriendId}");
            Console.WriteLine("**************************************************************************************************************");

            // Fetch student names based on their IDs
            var requester = await _studentsServiceClient.GetAsync(friendEvent.RequesterId);
            var friend = await _studentsServiceClient.GetAsync(friendEvent.FriendId);

            if (requester == null || friend == null)
            {
                _logger.LogError($"Failed to fetch student data for RequesterId={friendEvent.RequesterId} or FriendId={friendEvent.FriendId}");
                return; // Early exit if any student data is missing
            }

            var eventDetails = $"A new friend request created from {requester.FirstName} {requester.LastName} to {friend.FirstName} {friend.LastName}";
            var notificationMessage = $"You have received a friend request from {requester.FirstName} {requester.LastName}";

            var newFriendEvent = new FriendEvent(
                id: Guid.NewGuid(),
                eventId: Guid.NewGuid(),
                userId: friendEvent.RequesterId, 
                friendId: friendEvent.FriendId,
                eventType: "FriendRequestCreated",
                details: eventDetails,
                createdAt: DateTime.UtcNow
            );

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: friendEvent.FriendId, // Notifying the friend, not the requester
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            await _friendEventRepository.AddAsync(newFriendEvent);
            await _messageBroker.PublishAsync(friendEvent);

            _logger.LogInformation($"Stored new friend event for UserId={friendEvent.RequesterId} with details: {eventDetails}");
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
