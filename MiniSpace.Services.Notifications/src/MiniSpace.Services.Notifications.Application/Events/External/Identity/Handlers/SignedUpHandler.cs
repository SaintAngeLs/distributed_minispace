using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Paralax.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;

        public SignedUpHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken)
        {
            var userNotifications = await _studentNotificationsRepository.GetByUserIdAsync(@event.UserId) ?? new UserNotifications(@event.UserId);

            var welcomeMessage = $"Welcome to MiniSpace, {@event.FirstName} {@event.LastName}!";
            
            var verificationLink = $"https://localhost:5606/verify-email/{HttpUtility.UrlEncode(@event.Token)}/{HttpUtility.UrlEncode(@event.Email)}/{HttpUtility.UrlEncode(@event.HashedToken)}/verify";

            var detailsHtml = $@"
                <p>Dear {@event.FirstName},<br>
                Welcome to MiniSpace! Your account with the email {@event.Email} has been successfully created. 
                You have been registered as a {@event.Role}. We are excited to have you on board!</p><br><br>
                <p>Please verify your email address by clicking the link below:<br>
                <a href='{verificationLink}'>Verify Email</a></p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: welcomeMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.UserId, 
                eventType: NotificationEventType.UserSignUp,
                details: detailsHtml
            );

            userNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(userNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: @event.UserId,
                message: welcomeMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.UserSignUp.ToString(),
                relatedEntityId: @event.UserId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
