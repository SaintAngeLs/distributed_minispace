using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class PendingFriendRequestAcceptedHandler : IEventHandler<PendingFriendAccepted>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<PendingFriendRequestAcceptedHandler> _logger;

        public PendingFriendRequestAcceptedHandler(
            IStudentNotificationsRepository studentNotificationsRepository, 
            IMessageBroker messageBroker, 
            IStudentsServiceClient studentsServiceClient, 
            ILogger<PendingFriendRequestAcceptedHandler> logger)
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
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

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.RequesterId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(@event.RequesterId);
                _logger.LogInformation($"Creating new StudentNotifications for UserId={@event.RequesterId}");
            }

            studentNotifications.AddNotification(notification);
            _logger.LogInformation($"Adding notification to StudentNotifications for UserId={@event.RequesterId}");

            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);
            _logger.LogInformation($"Updated StudentNotifications for UserId={@event.RequesterId}");

         

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt,
                eventType: "FriendRequestAccepted",
                relatedEntityId: notification.RelatedEntityId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
            _logger.LogInformation($"Published enhanced NotificationCreated event for UserId={notification.UserId}");

        }
    }
}
