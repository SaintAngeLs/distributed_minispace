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

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class StudentCancelledSignUpToEventHandler : IEventHandler<StudentCancelledSignUpToEvent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<StudentCancelledSignUpToEventHandler> _logger;

        public StudentCancelledSignUpToEventHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<StudentCancelledSignUpToEventHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(StudentCancelledSignUpToEvent eventArgs, CancellationToken cancellationToken)
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

            var detailsHtml = $"<p>{student.FirstName} {student.LastName}, you have cancelled your registration for the event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";
            var notificationMessage = $"You have cancelled your registration for the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.StudentId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.StudentCancelledSignedUpToEvent,
                details: detailsHtml
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: eventArgs.StudentId,
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.StudentCancelledSignedUpToEvent.ToString(),
                relatedEntityId: eventArgs.EventId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = eventArgs.StudentId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.StudentCancelledSignedUpToEvent,
                RelatedEntityId = eventArgs.EventId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to student with ID {eventArgs.StudentId}.");

            // Notify the organizer
            var organizerNotificationMessage = $"{student.FirstName} {student.LastName} has cancelled their registration for your event '{eventDetails.Name}'.";
            var organizerDetailsHtml = $"<p>{student.FirstName} {student.LastName} has cancelled their registration for your event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";

            var organizerNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: organizerNotificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.StudentCancelledSignedUpToEvent,
                details: organizerDetailsHtml
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
                eventType: NotificationEventType.StudentCancelledSignedUpToEvent.ToString(),
                relatedEntityId: eventArgs.EventId,
                details: organizerDetailsHtml
            );

            await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);

            var organizerNotificationDto = new NotificationDto
            {
                UserId = eventDetails.Organizer.Id,
                Message = organizerNotificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.StudentCancelledSignedUpToEvent,
                RelatedEntityId = eventArgs.EventId,
                Details = organizerDetailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to organizer with ID {eventDetails.Organizer.Id}.");
        }
    }
}
