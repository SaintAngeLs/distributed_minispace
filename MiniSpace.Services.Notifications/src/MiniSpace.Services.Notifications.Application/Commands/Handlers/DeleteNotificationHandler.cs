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
            var exists = await _studentNotificationsRepository.NotificationExists(command.StudentId, command.NotificationId);
            if (!exists)
            {
                throw new NotificationNotFoundException(command.NotificationId);
            }

            // Assume that the Delete method in the repository takes care of marking it as deleted or actually removing it
            await _studentNotificationsRepository.DeleteAsync(command.StudentId, command.NotificationId);

            // Optional: handle domain events associated with deleting notifications
            // This could be publishing events that notification has been deleted etc.
            var events = _eventMapper.MapAll(new [] { "NotificationDeleted" }); // Example of mapping deletion to an event
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
