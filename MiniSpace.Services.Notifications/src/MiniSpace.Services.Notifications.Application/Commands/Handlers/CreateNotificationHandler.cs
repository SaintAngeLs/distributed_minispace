using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class CreateNotificationHandler : ICommandHandler<CreateNotification>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public CreateNotificationHandler(
            IStudentNotificationsRepository studentNotificationsRepository,
            IFriendsServiceClient friendsServiceClient,
             IEventsServiceClient eventsServiceClient,
            IEventMapper eventMapper, 
            IMessageBroker messageBroker
            )
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _friendsServiceClient = friendsServiceClient;
             _eventsServiceClient = eventsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateNotification command, CancellationToken cancellationToken = default)
        {
            var notification = new Notification(
                command.NotificationId, 
                command.UserId, 
                command.Message, 
                NotificationStatus.Unread,
                DateTime.UtcNow, 
                null,
                NotificationEventType.Other,
                command.UserId
                
            );
            await _notificationRepository.AddAsync(notification);

            var events = _eventMapper.MapAll(notification.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
