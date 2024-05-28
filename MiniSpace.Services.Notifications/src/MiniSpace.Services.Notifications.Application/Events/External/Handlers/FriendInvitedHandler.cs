using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System.Text.Json;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendInvitedHandler : IEventHandler<FriendInvited>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendInvitedHandler(
            INotificationRepository notificationRepository,
            IStudentNotificationsRepository studentNotificationsRepository,  
            IStudentsServiceClient studentsServiceClient, 
            IEventMapper eventMapper, 
            IMessageBroker messageBroker
        )
        {
            _notificationRepository = notificationRepository;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendInvited @event, CancellationToken cancellationToken)
        {
            var inviter = await _studentsServiceClient.GetAsync(@event.InviterId);
            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";
            var detailsHtml = $"<p>View <a href='https://minispace.itsharppro.com/user-details/{@event.InviterId}'>{inviter.FirstName} {inviter.LastName}</a>'s profile to respond to the friend invitation.</p>";

            var notificationId = Guid.NewGuid();
            var notification = new Notification(
                notificationId: notificationId,
                userId: @event.InviteeId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.InviterId,
                eventType: NotificationEventType.NewFriendRequest,
                details: detailsHtml
            );

            await _notificationRepository.AddAsync(notification);

              var notificationCreatedEvent = new NotificationCreated(
                notificationId,
                @event.InviteeId,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.NewFriendRequest.ToString(),
                @event.InviterId,
                detailsHtml
            );

            var serializedEvent = JsonSerializer.Serialize(notificationCreatedEvent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.InviteeId) ?? new StudentNotifications(@event.InviteeId);
            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);
        }

    }
}
