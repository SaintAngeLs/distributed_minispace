using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;
using System.Text.Json;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class NotificationCreatedHandler : IEventHandler<NotificationCreated>
    {
        private readonly IUserNotificationsRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public NotificationCreatedHandler(
            IUserNotificationsRepository notificationRepository, 
            IEventMapper eventMapper, 
            IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
        {
            // var notification = await _notificationRepository.GetByUserIdAsync(@event.UserId); 
            // // GetAsync(@event.NotificationId);
            // if (notification == null)
            // {
            //     throw new NotificationNotFoundException(@event.NotificationId);
            // }

            // await _notificationRepository.AddAsync(notification);

            // var events = _eventMapper.Map(notification);
            // Console.WriteLine($"Event Published in NOTIFICATION CREATED HANDLER: {JsonSerializer.Serialize(@event)}");

            // await _messageBroker.PublishAsync(@event);
        }
    }
}
