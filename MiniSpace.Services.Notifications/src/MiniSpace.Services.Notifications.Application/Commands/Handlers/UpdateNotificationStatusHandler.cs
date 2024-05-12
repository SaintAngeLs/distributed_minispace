using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Exceptions;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class UpdateNotificationStatusHandler : ICommandHandler<UpdateNotificationStatus>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateNotificationStatusHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateNotificationStatus command, CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.GetAsync(command.NotificationId);
            if (notification == null)
            {
                throw new NotificationNotFoundException(command.NotificationId);
            }

            if (Enum.TryParse<NotificationStatus>(command.Status, true, out var status))
            {
                switch (status)
                {
                    case NotificationStatus.Read:
                        notification.MarkAsRead();
                        break;
                    case NotificationStatus.Deleted:
                        notification.MarkAsDeleted();
                        break;
                }
            }
            else
            {
                throw new InvalidNotificationStatusException($"Invalid status: {command.Status}");
            }

            await _notificationRepository.UpdateAsync(notification);

            var events = _eventMapper.MapAll(notification.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
