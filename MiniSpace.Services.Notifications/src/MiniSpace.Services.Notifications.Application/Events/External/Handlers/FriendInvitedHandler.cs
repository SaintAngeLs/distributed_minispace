using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendInvitedHandler : IEventHandler<NotificationCreated>,  
                                        IEventHandler<FriendInvited>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendInvitedHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetAsync(@event.NotificationId);
            if (notification == null)
            {
                throw new NotificationNotFoundException(@event.NotificationId);
            }

            await _notificationRepository.AddAsync(notification);
            var events = _eventMapper.MapAll(notification.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        public async Task HandleAsync(FriendInvited @event, CancellationToken cancellationToken)
        {
            // Create a notification object based on the event details
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId, // Assuming the invitee should receive the notification
                message: $"You have been invited by user {@event.InviterId} to be friends.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            // Save the notification to the repository
            await _notificationRepository.AddAsync(notification);

            // Optionally, if there are any other domain events resulting from this, publish them
            // Here we can use the _messageBroker to publish any further events if required by the domain logic
            // For instance:
            // var additionalEvents = new[] { new NotificationEvent(notification.NotificationId, "NotificationCreated") };
            // await _messageBroker.PublishAsync(additionalEvents);
        }
    }
}
