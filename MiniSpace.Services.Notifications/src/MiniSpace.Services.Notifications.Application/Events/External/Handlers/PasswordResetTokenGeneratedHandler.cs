using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class PasswordResetTokenGeneratedHandler : IEventHandler<PasswordResetTokenGenerated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public PasswordResetTokenGeneratedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(PasswordResetTokenGenerated @event, CancellationToken cancellationToken)
        {
            var student = await _studentsServiceClient.GetAsync(@event.UserId);
            var encodedToken =  System.Net.WebUtility.UrlEncode(@event.ResetToken); 
            var resetLink = $"https://minispace.itsharppro.com/reset-password/{encodedToken}/page"; 
            var notificationMessage = $"You have requested to reset your password. Please use the following link to proceed: {resetLink}";
            var detailsHtml = $"<p>Click the following link to reset your password: <a href=\"{resetLink}\">Reset Password</a></p>";


            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.UserId, 
                eventType: NotificationEventType.PasswordResetRequest,
                details: detailsHtml
            );

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.UserId) ?? new StudentNotifications(@event.UserId);
            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                @event.UserId,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.PasswordResetRequest.ToString(),
                @event.UserId, 
                detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
