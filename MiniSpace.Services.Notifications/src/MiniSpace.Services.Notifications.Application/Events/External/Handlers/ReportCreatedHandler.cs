using Convey.CQRS.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReportCreatedHandler : IEventHandler<ReportCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IReportsServiceClient _reportsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public ReportCreatedHandler(
            IMessageBroker messageBroker,
            IReportsServiceClient reportsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _reportsServiceClient = reportsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(ReportCreated eventArgs, CancellationToken cancellationToken)
        {
            var report = await _reportsServiceClient.GetReportAsync(eventArgs.ReportId);
            if (report == null)
            {
                Console.WriteLine("Report not found.");
                return;
            }

            var targetOwnerNotification = await CreateNotificationForUser(report.TargetOwnerId, report, "A report has been created about your content.");
            await PublishAndSaveNotification(targetOwnerNotification, report.TargetOwnerId, "ReportCreated");
            
            var issuerNotification = await CreateNotificationForUser(report.IssuerId, report, "Thank you for submitting your report. We will review it promptly.");
            await PublishAndSaveNotification(issuerNotification, report.IssuerId, "ThankYouForReporting");
        }

        private async Task<Notification> CreateNotificationForUser(Guid userId, ReportDto report, string message)
        {
            var notifications = await _studentNotificationsRepository.GetByStudentIdAsync(userId) ?? new StudentNotifications(userId);
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: userId,
                message: message,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: report.Id,
                eventType: NotificationEventType.ReportCreated
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
