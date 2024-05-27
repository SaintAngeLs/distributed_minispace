using Convey.CQRS.Commands;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Core.Entities;
using MiniSpace.Services.Email.Application.Services;

namespace MiniSpace.Services.Email.Application.Commands.Handlers
{
    public class CreateNotificationHandler : ICommandHandler<CreateNotification>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public CreateNotificationHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
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
