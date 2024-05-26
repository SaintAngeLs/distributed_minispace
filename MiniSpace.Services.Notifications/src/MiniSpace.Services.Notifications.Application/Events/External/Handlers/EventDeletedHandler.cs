using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public EventDeletedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            IEnumerable<StudentDto> users;

            try
            {
                users = await _studentsServiceClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get users: {ex.Message}");
                throw;
            }

            if (users == null)
            {
                Console.WriteLine("No users found.");
                return;
            }

            foreach (var user in users)
            {
                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(user.Id);
                if (studentNotifications == null)
                {
                    continue; // If there are no notifications for this user, skip
                }

                // Remove notifications related to the deleted event
                await _studentNotificationsRepository.UpdateAsync(studentNotifications);

                Console.WriteLine($"Removed notifications for user: {user.Id} related to deleted event: {eventDeleted.EventId}");

                // Optionally, send a notification about the event cancellation
                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: user.Id,
                    message: $"An event has been cancelled.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventDeleted.EventId,
                    eventType: NotificationEventType.EventDeleted
                );

                studentNotifications.AddNotification(notification);
                await _studentNotificationsRepository.UpdateAsync(studentNotifications);

                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: notification.NotificationId,
                    userId: notification.UserId,
                    message: notification.Message,
                    createdAt: notification.CreatedAt
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }
        }
    }
}
