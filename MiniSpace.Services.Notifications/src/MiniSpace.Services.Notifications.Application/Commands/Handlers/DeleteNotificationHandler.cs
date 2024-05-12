using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services;
using System.Threading.Tasks;
using System;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class DeleteNotificationHandler : ICommandHandler<DeleteNotification>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public DeleteNotificationHandler(INotificationRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteNotification command, CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.GetAsync(command.NotificationId);
             if (notification == null)
            {
                throw new NotificationNotFoundException(command.NotificationId);
            }

            notification.MarkAsDeleted();

            await _notificationRepository.UpdateAsync(notification);

            var events = _eventMapper.MapAll(notification.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
