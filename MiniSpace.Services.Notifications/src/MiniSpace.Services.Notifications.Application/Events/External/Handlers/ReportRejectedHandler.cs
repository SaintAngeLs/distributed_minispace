using Convey.CQRS.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReportRejectedHandler : IEventHandler<ReportRejected>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public ReportRejectedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(ReportRejected eventArgs, CancellationToken cancellationToken)
        {
            // Notify the issuer that their report has been rejected
            var issuerNotification = await CreateNotificationForUser(eventArgs.IssuerId, eventArgs, "Your report has been rejected. Reason: " + eventArgs.Reason);
            await PublishAndSaveNotification(issuerNotification, eventArgs.IssuerId, "ReportRejected");
        }

        private async Task<Notification> CreateNotificationForUser(Guid userId, ReportRejected eventArgs, string message)
        {
            var notifications = await _studentNotificationsRepository.GetByStudentIdAsync(userId) ?? new StudentNotifications(userId);
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: userId,
                message: message,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.ReportId,
                eventType: NotificationEventType.ReportRejected
            );
            notifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(notifications);
            return notification;
        }

        private async Task PublishAndSaveNotification(Notification notification, Guid userId, string eventType)
        {
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt,
                eventType: eventType,
                relatedEntityId: notification.RelatedEntityId,
                details: $"Notification for user {userId}. Message: {notification.Message}"
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
