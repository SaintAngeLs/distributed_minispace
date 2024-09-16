using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<EventDeletedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventDeletedHandler(
            IEventsServiceClient eventsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<EventDeletedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _eventsServiceClient = eventsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch participants and interested users for the event
                var eventParticipants = await _eventsServiceClient.GetParticipantsAsync(eventDeleted.EventId);

                if (eventParticipants == null)
                {
                    _logger.LogWarning($"No participants found for event {eventDeleted.EventId}");
                    return;
                }

                // Combine signed-up users and interested users
                var allUsersToNotify = eventParticipants.SignedUpStudents
                    .Concat(eventParticipants.InterestedStudents)
                    .Distinct()
                    .Select(p => p.UserId)
                    .ToList();

                // Notify all relevant users
                foreach (var userId in allUsersToNotify)
                {
                    var notificationMessage = $"The event '{eventDeleted.EventName}' (Event ID: {eventDeleted.EventId}) scheduled from {eventDeleted.StartDate:yyyy-MM-dd} to {eventDeleted.EndDate:yyyy-MM-dd} has been cancelled.";

                    var notification = new Notification(
                        notificationId: Guid.NewGuid(),
                        userId: userId,
                        message: notificationMessage,
                        status: NotificationStatus.Unread,
                        createdAt: DateTime.UtcNow,
                        updatedAt: null,
                        relatedEntityId: eventDeleted.EventId,
                        eventType: NotificationEventType.EventDeleted
                    );

                    var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(userId)
                                          ?? new UserNotifications(userId);

                    userNotifications.AddNotification(notification);
                    await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

                    // Broadcast the notification to the user using SignalR
                    var notificationDto = new NotificationDto
                    {
                        UserId = userId,
                        Message = notificationMessage,
                        CreatedAt = notification.CreatedAt,
                        EventType = NotificationEventType.EventDeleted,
                        RelatedEntityId = eventDeleted.EventId,
                        Details = $"<p>The event '{eventDeleted.EventName}' has been cancelled.</p>"
                    };

                    await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to handle EventDeleted event: {ex.Message}");
            }
        }
    }
}
