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
            // Logic to handle a FriendRequestSent event
            // Example: create a notification to inform the invitee about the friend request
            var notificationMessage = $"You have received a friend request.";

            // Assuming Notification creation
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId, // the recipient of the notification
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            await _notificationRepository.AddAsync(notification);

            // Optionally, map and publish additional events if necessary
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt
            );

            // Publish the NotificationCreated event
            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}


// using Convey.CQRS.Events;
// using MiniSpace.Services.Notifications.Core.Repositories;
// using MiniSpace.Services.Notifications.Application.Services;
// using MiniSpace.Services.Notifications.Core.Entities;
// using System.Collections.Generic;
// using MiniSpace.Services.Notifications.Application.Exceptions;

// namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
// {
//     public class FriendRequestSentHandler : IEventHandler<NotificationCreated>
//     {
//         private readonly INotificationRepository _notificationRepository;
//         private readonly IEventMapper _eventMapper;
//         private readonly IMessageBroker _messageBroker;

//         public FriendRequestSentHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
//         {
//             _notificationRepository = notificationRepository;
//             _eventMapper = eventMapper;
//             _messageBroker = messageBroker;
//         }

//         public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
//         {
//             var notification = await _notificationRepository.GetAsync(@event.NotificationId);
//             if (notification == null)
//             {
//                 throw new NotificationNotFoundException(@event.NotificationId);
//             }

//             await _notificationRepository.AddAsync(notification);
//             var events = _eventMapper.MapAll(notification.Events);
//             await _messageBroker.PublishAsync(events.ToArray());
//         }
//     }
// }
