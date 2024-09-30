using Paralax.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Services;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;


namespace MiniSpace.Services.Notifications.Application.Events.External.Friends.Handlers
{
    public class PendingFriendRequestAcceptedHandler : IEventHandler<PendingFriendAccepted>
    {
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<PendingFriendRequestAcceptedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext; 

        public PendingFriendRequestAcceptedHandler(
            IUserNotificationsRepository studentNotificationsRepository, 
            IMessageBroker messageBroker, 
            IStudentsServiceClient studentsServiceClient, 
            ILogger<PendingFriendRequestAcceptedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(PendingFriendAccepted @event, CancellationToken cancellationToken)
        {
            var requester = await _studentsServiceClient.GetAsync(@event.RequesterId);
            var friend = await _studentsServiceClient.GetAsync(@event.FriendId);

            if (requester == null || friend == null)
            {
                _logger.LogError($"Failed to fetch student data for RequesterId={@event.RequesterId} or FriendId={@event.FriendId}");
                return; 
            }

            var notificationMessage = $"Your friend request to {friend.FirstName} {friend.LastName} has been accepted.";
            var detailsHtml = $"<p>Your friend request with <a href='https://minispace.itsharppro.com/student-details/{@event.FriendId}'>{friend.FirstName} {friend.LastName}</a> has been accepted.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.RequesterId,  
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.FriendId,
                eventType: NotificationEventType.FriendRequestAccepted,
                details: detailsHtml
            );

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(@event.RequesterId);
            if (studentNotifications == null)
            {
                studentNotifications = new UserNotifications(@event.RequesterId);
                _logger.LogInformation($"Creating new StudentNotifications for UserId={@event.RequesterId}");
            }

            studentNotifications.AddNotification(notification);
            _logger.LogInformation($"Adding notification to StudentNotifications for UserId={@event.RequesterId}");

            var notificationCreatedEvent = new NotificationCreated(
                notificationId:  Guid.NewGuid(),
                userId: @event.RequesterId, 
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.FriendRequestAccepted.ToString(),
                relatedEntityId: @event.FriendId,
                details: detailsHtml
            );

            var notificationDto = new NotificationDto
            {
                UserId = @event.RequesterId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.FriendRequestAccepted,
                RelatedEntityId = @event.FriendId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation("Broadcasted SignalR notification to all users.");

            await _messageBroker.PublishAsync(notificationCreatedEvent);
            _logger.LogInformation($"Published enhanced NotificationCreated event for UserId={notification.UserId}");

            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);
            _logger.LogInformation($"Updated StudentNotifications for UserId={@event.RequesterId}");
        }
    }
}
