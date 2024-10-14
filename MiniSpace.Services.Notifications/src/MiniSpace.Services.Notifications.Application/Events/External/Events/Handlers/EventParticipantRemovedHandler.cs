using System;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events.Handlers
{
    public class EventParticipantRemovedHandler : IEventHandler<EventParticipantRemoved>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly ILogger<EventParticipantRemovedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventParticipantRemovedHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            ILogger<EventParticipantRemovedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventParticipantRemoved eventArgs, CancellationToken cancellationToken)
        {
            var participantNotifications = await _studentNotificationsRepository.GetByUserIdAsync(eventArgs.Participant);
            var participant = await _studentsServiceClient.GetAsync(eventArgs.Participant);
            if (participantNotifications == null)
            {
                participantNotifications = new UserNotifications(eventArgs.Participant);
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.EventId);
            var detailsHtml = eventDetails != null ?
                $"<p>You have been removed from the event '{eventDetails.Name}' scheduled on {eventDetails.StartDate:yyyy-MM-dd}.</p>" :
                "<p>Event details could not be retrieved.</p>";

            var notificationMessage = $"You have been removed from the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.Participant,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.EventParticipantRemoved,
                details: detailsHtml
            );

            participantNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(participantNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                eventArgs.Participant,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.EventParticipantRemoved.ToString(),
                eventArgs.EventId,
                detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = eventArgs.Participant,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.EventParticipantRemoved,
                RelatedEntityId = eventArgs.EventId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to participant with ID {eventArgs.Participant}.");
        }
    }
}
