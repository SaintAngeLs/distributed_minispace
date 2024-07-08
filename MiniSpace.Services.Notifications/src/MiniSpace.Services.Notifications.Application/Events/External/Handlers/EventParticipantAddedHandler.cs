using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventParticipantAddedHandler : IEventHandler<EventParticipantAdded>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly ILogger<EventParticipantAddedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventParticipantAddedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            ILogger<EventParticipantAddedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventParticipantAdded eventArgs, CancellationToken cancellationToken)
        {
            var participantNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventArgs.ParticipantId);
            var participant = await _studentsServiceClient.GetAsync(eventArgs.ParticipantId);
            if (participantNotifications == null)
            {
                participantNotifications = new StudentNotifications(eventArgs.ParticipantId);
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.EventId);
            var detailsHtml = eventDetails != null ?
                $"<p>{eventArgs.ParticipantName}, you have been added as a participant to the event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>" :
                "<p>Event details could not be retrieved.</p>";

            var notificationMessage = $"You have been added as a participant to the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.ParticipantId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.EventParticipantAdded,
                details: detailsHtml
            );

            participantNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(participantNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt,
                eventType: notification.EventType.ToString(),
                relatedEntityId: notification.RelatedEntityId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = eventArgs.ParticipantId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.EventParticipantAdded,
                RelatedEntityId = eventArgs.EventId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to participant with ID {eventArgs.ParticipantId}.");

            if (eventDetails != null && eventDetails.Organizer != null)
            {
                var detailsHtmlForOrganizer = $"<p>{eventArgs.ParticipantName} has been added as a participant to your event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";
                var organizerNotification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: eventDetails.Organizer.Id,
                    message: $"{eventArgs.ParticipantName} has been added as a participant to your event '{eventDetails.Name}'.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventArgs.EventId,
                    eventType: NotificationEventType.EventParticipantAdded,
                    details: detailsHtmlForOrganizer
                );

                var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventDetails.Organizer.Id);
                if (organizerNotifications == null)
                {
                    organizerNotifications = new StudentNotifications(eventDetails.Organizer.Id);
                }

                organizerNotifications.AddNotification(organizerNotification);
                await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

                var organizerNotificationCreatedEvent = new NotificationCreated(
                    notificationId: organizerNotification.NotificationId,
                    userId: eventDetails.Organizer.Id,
                    message: organizerNotification.Message,
                    createdAt: DateTime.UtcNow,
                    eventType: NotificationEventType.EventParticipantAdded.ToString(),
                    relatedEntityId: eventArgs.EventId,
                    details: detailsHtmlForOrganizer
                );

                await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);

                var organizerNotificationDto = new NotificationDto
                {
                    UserId = eventDetails.Organizer.Id,
                    Message = organizerNotification.Message,
                    CreatedAt = DateTime.UtcNow,
                    EventType = NotificationEventType.EventParticipantAdded,
                    RelatedEntityId = eventArgs.EventId,
                    Details = detailsHtmlForOrganizer
                };

                await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
                _logger.LogInformation($"Broadcasted SignalR notification to organizer with ID {eventDetails.Organizer.Id}.");
            }
        }
    }
}
