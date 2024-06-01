using Convey.CQRS.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReportResolvedHandler : IEventHandler<ReportResolved>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public ReportResolvedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(ReportResolved eventArgs, CancellationToken cancellationToken)
        {
            // Fetch student details
            var issuer = await _studentsServiceClient.GetAsync(eventArgs.IssuerId);
            var reviewer = eventArgs.ReviewerId.HasValue ? await _studentsServiceClient.GetAsync(eventArgs.ReviewerId.Value) : null;

            string issuerName = $"{issuer.FirstName} {issuer.LastName}";
            string reviewerName = reviewer != null ? $"{reviewer.FirstName} {reviewer.LastName}" : "Reviewer";

            // Detailed notification for issuer
            string issuerMessage = $"Dear {issuerName}, your report about '{eventArgs.Category}' concerning '{eventArgs.ContextType}' has been resolved.";
            var issuerNotification = await CreateNotificationForUser(eventArgs.IssuerId, eventArgs, issuerMessage);
            await PublishAndSaveNotification(issuerNotification, eventArgs.IssuerId, "ReportResolved", issuerName);

            // Detailed notification for reviewer if applicable
            if (eventArgs.ReviewerId.HasValue)
            {
                string reviewerMessage = $"Dear {reviewerName}, a report you reviewed about '{eventArgs.Category}' concerning '{eventArgs.ContextType}' has been resolved.";
                var reviewerNotification = await CreateNotificationForUser(eventArgs.ReviewerId.Value, eventArgs, reviewerMessage);
                await PublishAndSaveNotification(reviewerNotification, eventArgs.ReviewerId.Value, "ReportResolved", reviewerName);
            }
        }

        private async Task<Notification> CreateNotificationForUser(Guid userId, ReportResolved eventArgs, string message)
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
                eventType: NotificationEventType.ReportResolved
            );
            notifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(notifications);
            return notification;
        }

        private async Task PublishAndSaveNotification(Notification notification, Guid userId, string eventType, string userName)
        {
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: $"{userName}, {notification.Message}",
                createdAt: notification.CreatedAt,
                eventType: NotificationEventType.ReportResolved.ToString(),
                relatedEntityId: notification.RelatedEntityId,
                details: $"Notification for user {userId} ({userName}). Message: {notification.Message}"
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
