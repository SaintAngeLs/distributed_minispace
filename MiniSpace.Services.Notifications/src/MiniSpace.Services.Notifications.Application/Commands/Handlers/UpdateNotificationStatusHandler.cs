using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers {
    public class UpdateNotificationStatusHandler : ICommandHandler<UpdateNotificationStatus> {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public UpdateNotificationStatusHandler(IStudentNotificationsRepository studentNotificationsRepository) {
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(UpdateNotificationStatus command, CancellationToken cancellationToken = default)
        {
            var notificationExists = await _studentNotificationsRepository.NotificationExists(command.UserId, command.NotificationId);
            if (!notificationExists)
                throw new NotificationNotFoundException(command.NotificationId);

            if (Enum.TryParse<NotificationStatus>(command.Status, true, out var newStatus))
            {
                await _studentNotificationsRepository.UpdateNotificationStatus(command.UserId, command.NotificationId, newStatus.ToString());
            }
            else
                throw new ArgumentException($"Invalid status value: {command.Status}");
        }

    }
}
