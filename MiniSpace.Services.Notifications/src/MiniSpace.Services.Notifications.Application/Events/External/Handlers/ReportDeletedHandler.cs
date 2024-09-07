using System;
using System.Threading.Tasks;
using System.Threading;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReportDeletedHandler : IEventHandler<ReportDeleted>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<ReportDeletedHandler> _logger;

        public ReportDeletedHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<ReportDeletedHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(ReportDeleted eventArgs, CancellationToken cancellationToken)
        {
            var issuer = await _studentsServiceClient.GetAsync(eventArgs.IssuerId);
            var targetOwner = await _studentsServiceClient.GetAsync(eventArgs.TargetOwnerId);

            if (issuer == null || targetOwner == null)
            {
                _logger.LogError("Issuer or target owner details not found.");
                return;
            }

            string issuerName = $"{issuer.FirstName} {issuer.LastName}";
            string targetOwnerName = $"{targetOwner.FirstName} {targetOwner.LastName}";

            // Detailed notification for issuer
            string issuerMessage = $"Dear {issuerName}, the report you filed about '{eventArgs.Category}' concerning '{eventArgs.ContextType}' has been deleted.";
            var issuerNotification = await CreateNotificationForUser(eventArgs.IssuerId, eventArgs, issuerMessage);
            await PublishAndSaveNotification(issuerNotification, eventArgs.IssuerId, issuerMessage);

            // Detailed notification for target owner
            string targetOwnerMessage = $"Hello {targetOwnerName}, a report about '{eventArgs.Category}' concerning your content '{eventArgs.ContextType}' has been deleted.";
            var targetOwnerNotification = await CreateNotificationForUser(eventArgs.TargetOwnerId, eventArgs, targetOwnerMessage);
            await PublishAndSaveNotification(targetOwnerNotification, eventArgs.TargetOwnerId, targetOwnerMessage);
        }

        private async Task<Notification> CreateNotificationForUser(Guid userId, ReportDeleted eventArgs, string message)
        {
            var notifications = await _studentNotificationsRepository.GetByUserIdAsync(userId) ?? new UserNotifications(userId);
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: userId,
                message: message,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.ReportId,
                eventType: NotificationEventType.ReportDeleted
            );
            notifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(notifications);
            return notification;
        }

        private async Task PublishAndSaveNotification(Notification notification, Guid userId, string message)
        {
            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                notification.UserId,
                message,
                notification.CreatedAt,
                NotificationEventType.ReportDeleted.ToString(),
                notification.RelatedEntityId,
                $"Notification for user {userId}. Message: {message}"
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = notification.UserId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.ReportDeleted,
                RelatedEntityId = notification.RelatedEntityId,
                Details = $"Notification for user {userId}. Message: {message}"
            };

            // Broadcast SignalR notification
            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to user with ID {userId}.");
        }
    }
}
