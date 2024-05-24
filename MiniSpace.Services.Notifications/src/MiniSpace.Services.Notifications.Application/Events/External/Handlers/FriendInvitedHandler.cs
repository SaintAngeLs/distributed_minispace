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
            // var invitee = await _studentsServiceClient.GetAsync(@event.InviteeId);
       
            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";

             var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId, 
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.InviterId,
                eventType: NotificationEventType.NewFriendRequest
            );

            await _notificationRepository.AddAsync(notification);

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(@event.InviteeId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(@event.InviteeId);
            }
            else
            {
                // _logger.AddInformation($"Retrieved existing notifications for studentId {@event.InviterId}.");
            }

            studentNotifications.AddNotification(notification);

            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
