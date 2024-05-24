using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Exceptions;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestSentHandler : IEventHandler<FriendRequestSent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendRequestSentHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendRequestSent @event, CancellationToken cancellationToken)
        {
            var notificationMessage = $"You have received a friend request.";

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
