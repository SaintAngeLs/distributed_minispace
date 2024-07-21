using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReportCancelledHandler : IEventHandler<ReportCancelled>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<ReportCancelledHandler> _logger;

        public ReportCancelledHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<ReportCancelledHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(ReportCancelled eventArgs, CancellationToken cancellationToken)
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

            // Notification message for issuer
            string issuerMessage = $"Your report about '{eventArgs.Category}' concerning '{eventArgs.ContextType}' has been successfully cancelled.";
            var issuerNotification = await CreateNotificationForUser(eventArgs.IssuerId, eventArgs, issuerMessage);
            await PublishAndSaveNotification(issuerNotification, eventArgs.IssuerId, issuerMessage);
            
            // Notification message for target owner
            string targetOwnerMessage = $"A report about '{eventArgs.Category}' concerning your content '{eventArgs.ContextType}' has been cancelled.";
            var targetOwnerNotification = await CreateNotificationForUser(eventArgs.TargetOwnerId, eventArgs, targetOwnerMessage);
            await PublishAndSaveNotification(targetOwnerNotification, eventArgs.TargetOwnerId, targetOwnerMessage);
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
                NotificationEventType.ReportCancelled.ToString(),
                notification.RelatedEntityId,
                $"Notification for user {userId}. Message: {message}"
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = notification.UserId,
                Message = message,
                CreatedAt = notification.CreatedAt,
                EventType = NotificationEventType.ReportCancelled,
                RelatedEntityId = notification.RelatedEntityId,
                Details = $"Notification for user {userId}. Message: {message}"
            };

            // Broadcast SignalR notification
            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to user with ID {userId}.");
        }
    }
}
