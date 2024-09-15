using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class NotificationUpdatedHandler : IEventHandler<NotificationUpdated>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper; 

        public NotificationUpdatedHandler(INotificationRepository notificationRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _notificationRepository = notificationRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(NotificationUpdated @event, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetAsync(@event.NotificationId);
            if (notification == null)
            {
                throw new NotificationNotFoundException(@event.NotificationId);
            }

            switch (@event.NewStatus)
            {
                case NotificationStatus.Read:
                    notification.MarkAsRead();
                    break;
                case NotificationStatus.Deleted:
                    notification.MarkAsDeleted();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported status update.");
            }

            await _notificationRepository.UpdateAsync(notification);

            var events = _eventMapper.MapAll(notification.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
