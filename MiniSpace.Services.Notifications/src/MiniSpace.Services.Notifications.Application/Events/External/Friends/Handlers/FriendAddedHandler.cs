using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;
// This event handler is not used! ⁉️
namespace MiniSpace.Services.Notifications.Application.Events.External.Friends.Handlers
{
    public class FriendAddedHandler : IEventHandler<NotificationCreated>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendAddedHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
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
    }
}
