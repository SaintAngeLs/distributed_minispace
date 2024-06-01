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
    public class ReportCancelledHandler : IEventHandler<ReportCancelled>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public ReportCancelledHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(ReportCancelled eventArgs, CancellationToken cancellationToken)
        {
            var issuer = await _studentsServiceClient.GetAsync(eventArgs.IssuerId);
            var targetOwner = await _studentsServiceClient.GetAsync(eventArgs.TargetOwnerId);

            string issuerName = $"{issuer.FirstName} {issuer.LastName}";
            string targetOwnerName = $"{targetOwner.FirstName} {targetOwner.LastName}";

            // Notification message for issuer with more details
            string issuerMessage = $"Your report about '{eventArgs.Category}' concerning '{eventArgs.ContextType}' has been successfully cancelled.";
            var issuerNotification = await CreateNotificationForUser(eventArgs.IssuerId, eventArgs, issuerMessage);
            await PublishAndSaveNotification(issuerNotification, eventArgs.IssuerId, "ReportCancellationConfirmed", issuerName);
            
            // Notification message for target owner with more details
            string targetOwnerMessage = $"A report about '{eventArgs.Category}' concerning your content '{eventArgs.ContextType}' has been cancelled.";
            var targetOwnerNotification = await CreateNotificationForUser(eventArgs.TargetOwnerId, eventArgs, targetOwnerMessage);
            await PublishAndSaveNotification(targetOwnerNotification, eventArgs.TargetOwnerId, "ReportCancelled", targetOwnerName);
        }

         private async Task<Notification> CreateNotificationForUser(Guid userId, ReportCancelled eventArgs, string message)
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
                eventType: NotificationEventType.ReportCancelled
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
                eventType: NotificationEventType.ReportCancelled.ToString(),
                relatedEntityId: notification.RelatedEntityId,
                details: $"Notification for user {userId} ({userName}). Message: {notification.Message}"
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
