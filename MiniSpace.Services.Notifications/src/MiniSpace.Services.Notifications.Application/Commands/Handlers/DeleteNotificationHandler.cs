using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services;
using System.Threading.Tasks;
using System;
using MiniSpace.Services.Notifications.Application.Events.External;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class DeleteNotificationHandler : ICommandHandler<DeleteNotification>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public DeleteNotificationHandler(IStudentNotificationsRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentNotificationsRepository = notificationRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteNotification command, CancellationToken cancellationToken = default)
        {
            var exists = await _studentNotificationsRepository.NotificationExists(command.UserId, command.NotificationId);
            if (!exists)
            {
                throw new NotificationNotFoundException(command.NotificationId);
            }

            await _studentNotificationsRepository.DeleteNotification(command.UserId, command.NotificationId);
          
            var notificationDeletedEvent = new NotificationDeleted(command.UserId, command.NotificationId);
            await _messageBroker.PublishAsync(notificationDeletedEvent);
        }
    }
}
