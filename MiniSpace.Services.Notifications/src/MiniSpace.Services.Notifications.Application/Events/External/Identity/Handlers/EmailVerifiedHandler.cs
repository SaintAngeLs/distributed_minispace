using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity.Handlers
{
    public class EmailVerifiedHandler : IEventHandler<EmailVerified>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;

        public EmailVerifiedHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EmailVerified @event, CancellationToken cancellationToken)
        {
            var userNotifications = await _studentNotificationsRepository.GetByUserIdAsync(@event.UserId) 
                                     ?? new UserNotifications(@event.UserId);

            var emailVerifiedMessage = $"Your email {@event.Email} has been successfully verified!";
            
            var detailsHtml = $@"
                <p>Dear User,<br>
                Congratulations! Your email address {@event.Email} has been successfully verified on { @event.VerifiedAt}.</p><br><br>
                <p>Thank you for verifying your email address.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: emailVerifiedMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.UserId,
                eventType: NotificationEventType.EmailVerified,
                details: detailsHtml
            );

            userNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(userNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: emailVerifiedMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.EmailVerified.ToString(),
                relatedEntityId: @event.UserId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
