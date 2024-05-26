using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventsServiceClient _eventsServiceClient;  
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public EventDeletedHandler(
            IMessageBroker messageBroker,
            IEventsServiceClient eventsServiceClient,  
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _eventsServiceClient = eventsServiceClient;  
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            IEnumerable<StudentDto> participants;
            try
            {
                participants = await _eventsServiceClient.GetParticipantsAsync(eventDeleted.EventId);  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get participants: {ex.Message}");
                throw;
            }

            if (participants == null)
            {
                Console.WriteLine("No participants found for the event.");
                return;
            }

            foreach (var participant in participants)
            {
                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: participant.Id,
                    message: $"The event you were signed up for has been cancelled.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventDeleted.EventId,
                    eventType: NotificationEventType.EventDeleted
                );

                Console.WriteLine($"Creating cancellation notification for user: {participant.Id}");

                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(participant.Id);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(participant.Id);
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
