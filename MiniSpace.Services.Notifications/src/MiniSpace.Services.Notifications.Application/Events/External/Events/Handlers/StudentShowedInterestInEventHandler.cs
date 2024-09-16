using System;
using System.Threading.Tasks;
using System.Threading;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events.Handlers
{
    public class StudentShowedInterestInEventHandler : IEventHandler<StudentShowedInterestInEvent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<StudentShowedInterestInEventHandler> _logger;

        public StudentShowedInterestInEventHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<StudentShowedInterestInEventHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(StudentShowedInterestInEvent eventArgs, CancellationToken cancellationToken)
        {
            var student = await _studentsServiceClient.GetAsync(eventArgs.StudentId);
            if (student == null)
            {
                _logger.LogError($"Student details not found for StudentId={eventArgs.StudentId}");
                return;
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.EventId);
            if (eventDetails == null)
            {
                _logger.LogError($"Event details not found for EventId={eventArgs.EventId}");
                return;
            }

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(eventArgs.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new UserNotifications(eventArgs.StudentId);
            }

            var detailsHtml = $"<p>{student.FirstName} {student.LastName}, you have shown interest in the event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";
            var notificationMessage = $"You have shown interest in the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.StudentId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.StudentShowedInterestInEvent,
                details: detailsHtml
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: eventArgs.StudentId,
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.StudentShowedInterestInEvent.ToString(),
                relatedEntityId: eventArgs.EventId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = eventArgs.StudentId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.StudentShowedInterestInEvent,
                RelatedEntityId = eventArgs.EventId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to student with ID {eventArgs.StudentId}.");

            if (eventDetails.Organizer != null)
            {
                var detailsHtmlForOrganizer = $"<p>{student.FirstName} {student.LastName} has shown interest in your event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";
                var organizerNotificationMessage = $"{student.FirstName} {student.LastName} has shown interest in your event '{eventDetails.Name}'.";

                var organizerNotification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: eventDetails.Organizer.Id,
                    message: organizerNotificationMessage,
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventArgs.EventId,
                    eventType: NotificationEventType.StudentShowedInterestInEvent,
                    details: detailsHtmlForOrganizer
                );

                var organizerNotifications = await _studentNotificationsRepository.GetByUserIdAsync(eventDetails.Organizer.Id);
                if (organizerNotifications == null)
                {
                    organizerNotifications = new UserNotifications(eventDetails.Organizer.Id);
                }

                organizerNotifications.AddNotification(organizerNotification);
                await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

                var organizerNotificationCreatedEvent = new NotificationCreated(
                    notificationId: organizerNotification.NotificationId,
                    userId: eventDetails.Organizer.Id,
                    message: organizerNotificationMessage,
                    createdAt: DateTime.UtcNow,
                    eventType: NotificationEventType.StudentShowedInterestInEvent.ToString(),
                    relatedEntityId: eventArgs.EventId,
                    details: detailsHtmlForOrganizer
                );

                await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);

                var organizerNotificationDto = new NotificationDto
                {
                    UserId = eventDetails.Organizer.Id,
                    Message = organizerNotificationMessage,
                    CreatedAt = DateTime.UtcNow,
                    EventType = NotificationEventType.StudentShowedInterestInEvent,
                    RelatedEntityId = eventArgs.EventId,
                    Details = detailsHtmlForOrganizer
                };

                await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
                _logger.LogInformation($"Broadcasted SignalR notification to organizer with ID {eventDetails.Organizer.Id}.");
            }
        }
    }
}
