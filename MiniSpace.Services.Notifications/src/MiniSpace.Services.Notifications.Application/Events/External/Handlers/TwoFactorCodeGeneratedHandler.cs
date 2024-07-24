using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class TwoFactorCodeGeneratedHandler : IEventHandler<TwoFactorCodeGenerated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<TwoFactorCodeGeneratedHandler> _logger;

        public TwoFactorCodeGeneratedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            ILogger<TwoFactorCodeGeneratedHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
        }

        public async Task HandleAsync(TwoFactorCodeGenerated @event, CancellationToken cancellationToken)
        {
            var student = await _studentsServiceClient.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogError($"Student with ID {@event.UserId} not found.");
                return;
            }

            var userNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.UserId) 
                                    ?? new StudentNotifications(@event.UserId);

            var notificationMessage = $"Your 2FA code is {@event.Code}. Please use this to complete your sign-in.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.UserId, 
                eventType: NotificationEventType.TwoFactorCodeGenerated,
                details: notificationMessage
            );

            // userNotifications.AddNotification(notification);
            // await _studentNotificationsRepository.AddOrUpdateAsync(userNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: @event.UserId,
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.TwoFactorCodeGenerated.ToString(),
                relatedEntityId: @event.UserId,
                details: notificationMessage
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
