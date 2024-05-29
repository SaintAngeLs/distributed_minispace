using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System.Text.Json;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestSentHandler : IEventHandler<FriendRequestSent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public FriendRequestSentHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(FriendRequestSent @event, CancellationToken cancellationToken)
        {
            var inviter = await _studentsServiceClient.GetAsync(@event.InviterId);
            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";
            var detailsHtml = $"<p>View <a href='https://minispace.itsharppro.com/user-details/{@event.InviterId}'>{inviter.FirstName} {inviter.LastName}</a>'s profile to respond to the friend invitation.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.InviterId,
                eventType: NotificationEventType.NewFriendRequest,
                details: detailsHtml
            );

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.InviteeId) ?? new StudentNotifications(@event.InviteeId);
            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                @event.InviteeId,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.NewFriendRequest.ToString(),
                @event.InviterId,
                detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
