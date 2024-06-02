using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public SignedUpHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

         public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken)
        {
            var userNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.UserId) ?? new StudentNotifications(@event.UserId);

            var welcomeMessage = $"Welcome to MiniSpace, {@event.FirstName} {@event.LastName}!";
            var detailsHtml = $"<p>Dear {@event.FirstName},<br>Welcome to MiniSpace! Your account with the email {@event.Email} has been successfully created. You have been registered as a {@event.Role}. We are excited to have you on board!</p>";

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
            await _studentNotificationsRepository.UpdateAsync(userNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId:  @event.UserId,
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
