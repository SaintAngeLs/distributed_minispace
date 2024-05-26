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
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public EventCreatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EventCreated eventCreated, CancellationToken cancellationToken)
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
                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: user.Id,
                    message: $"A new event has been created by Organizer {eventCreated.OrganizerId}",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventCreated.EventId,
                    eventType: NotificationEventType.NewEvent
                );

                Console.WriteLine($"Creating notification for user: {user.Id}");

                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(user.Id);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(user.Id);
                }

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

