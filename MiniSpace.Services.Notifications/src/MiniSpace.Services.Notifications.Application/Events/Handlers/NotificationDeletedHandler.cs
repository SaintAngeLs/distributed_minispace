using Paralax.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Exceptions;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class NotificationDeletedHandler : IEventHandler<NotificationDeleted>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper; 

        public NotificationDeletedHandler(INotificationRepository notificationRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _notificationRepository = notificationRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper; 
        }

        public async Task HandleAsync(NotificationDeleted @event, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetAsync(@event.NotificationId);
            
            if (notification == null)
            {
                throw new NotificationNotFoundException(@event.NotificationId);
            }

            notification.MarkAsDeleted();
            await _notificationRepository.UpdateAsync(notification);

            var events = _eventMapper.MapAll(notification.Events); 
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
