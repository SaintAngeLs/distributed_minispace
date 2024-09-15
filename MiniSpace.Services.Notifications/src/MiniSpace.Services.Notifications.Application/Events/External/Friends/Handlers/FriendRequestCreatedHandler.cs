using System;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Threading;

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
        private readonly IHubContext<NotificationHub> _hubContext;

        public FriendRequestCreatedHandler(
            IFriendEventRepository friendEventRepository, 
            IStudentsServiceClient studentsServiceClient, 
            IEventMapper eventMapper, 
            IMessageBroker messageBroker, 
            ILogger<FriendRequestCreatedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _friendEventRepository = friendEventRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(FriendRequestCreated friendEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Received FriendRequestCreated event: RequesterId={friendEvent.RequesterId}, FriendId={friendEvent.FriendId}");

            var requester = await _studentsServiceClient.GetAsync(friendEvent.RequesterId);
            var friend = await _studentsServiceClient.GetAsync(friendEvent.FriendId);

            if (requester == null || friend == null)
            {
                _logger.LogError($"Failed to fetch student data for RequesterId={friendEvent.RequesterId} or FriendId={friendEvent.FriendId}");
                return; 
            }

            var eventDetails = $"A new friend request created from {requester.FirstName} {requester.LastName} to {friend.FirstName} {friend.LastName}";
            var notificationMessage = $"You have received a friend request from {requester.FirstName} {requester.LastName}";
            var detailsHtml = $"<p>Click <a href='https://minispace.itsharppro.com/friend-requests'>here</a> to view the request.</p>";

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
                userId: friendEvent.FriendId, 
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: friendEvent.RequesterId, 
                eventType: NotificationEventType.NewFriendRequest,
                details: detailsHtml
            );

            var notificationDto = new NotificationDto
            {
                UserId = friendEvent.FriendId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.NewFriendRequest,
                RelatedEntityId = friendEvent.RequesterId,
                Details = detailsHtml
            };

            await NotificationHub.SendNotification(_hubContext, friendEvent.FriendId.ToString(), notificationDto, _logger);
            _logger.LogInformation($"Sent SignalR notification to UserId={friendEvent.FriendId}");

            await _friendEventRepository.AddAsync(newFriendEvent);
            _logger.LogInformation($"Stored new friend event for UserId={friendEvent.RequesterId} with details: {eventDetails}");

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: friendEvent.FriendId,
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.NewFriendRequest.ToString(),
                relatedEntityId: friendEvent.RequesterId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
            _logger.LogInformation($"Published NotificationCreated event for NotificationId={notification.NotificationId}");
        }
    }
}
