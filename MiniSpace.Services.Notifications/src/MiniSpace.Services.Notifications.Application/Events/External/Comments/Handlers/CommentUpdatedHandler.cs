using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Events.External.Comments;
using MiniSpace.Services.Notifications.Application.Dto.Events;
using MiniSpace.Services.Notifications.Application.Dto.Comments;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class CommentUpdatedHandler : IEventHandler<CommentUpdated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly ICommentsServiceClient _commentsServiceClient;
        private readonly ILogger<CommentUpdatedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentUpdatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IUserNotificationsRepository studentNotificationsRepository,
            ICommentsServiceClient commentsServiceClient,
            ILogger<CommentUpdatedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
            _commentsServiceClient = commentsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(CommentUpdated eventArgs, CancellationToken cancellationToken)
        {
            CommentDto commentDetails;
            try
            {
                commentDetails = await _commentsServiceClient.GetCommentAsync(eventArgs.CommentId);
                if (commentDetails == null)
                {
                    _logger.LogError("Updated comment details not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve updated comment details: {ex.Message}");
                throw;
            }

            EventDto eventDetails;
            try
            {
                eventDetails = await _eventsServiceClient.GetEventAsync(commentDetails.ContextId);
                if (eventDetails == null)
                {
                    _logger.LogError("Event details for comment context not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve event details for comment context: {ex.Message}");
                throw;
            }

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(commentDetails.UserId);
            if (studentNotifications == null)
            {
                studentNotifications = new UserNotifications(commentDetails.UserId);
            }

            // Create notification for the user who updated the comment
            var userNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: commentDetails.UserId,
                message: $"Your updated comment on the event '{eventDetails.Name}' has been processed.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentUpdated
            );

            studentNotifications.AddNotification(userNotification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var userNotificationDetailsHtml = $"<p>Your updated comment on the event '{eventDetails.Name}' is now visible.</p>";

            var notificationUpdatedEvent = new NotificationCreated(
                notificationId: userNotification.NotificationId,
                userId: userNotification.UserId,
                message: userNotification.Message,
                createdAt: userNotification.CreatedAt,
                eventType: "CommentUpdated",
                relatedEntityId: userNotification.RelatedEntityId,
                details: userNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationUpdatedEvent);

            // Broadcast the updated notification via SignalR
            var notificationDto = new NotificationDto
            {
                UserId = commentDetails.UserId,
                Message = userNotification.Message,
                CreatedAt = userNotification.CreatedAt,
                EventType = NotificationEventType.CommentUpdated,
                RelatedEntityId = eventArgs.CommentId,
                Details = userNotificationDetailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation("Broadcasted SignalR notification to the user.");

            // Organizer notification
            var organizerNotifications = await _studentNotificationsRepository.GetByUserIdAsync(eventDetails.Organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new UserNotifications(eventDetails.Organizer.Id);
            }

            // Create notification for the organizer
            var organizerNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"{eventArgs.UserName} has updated their comment on your event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentUpdated
            );

            organizerNotifications.AddNotification(organizerNotification);
            await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

            // Prepare and send organizer notification details HTML, including profile image
            var organizerNotificationDetailsHtml = $"<p>{eventArgs.UserName} updated their comment on your event '{eventDetails.Name}': {eventArgs.CommentContent}.</p>" +
                                                   $"<img src='{eventArgs.ProfileImageUrl}' alt='Profile Image' style='width:50px;height:50px;' />";

            var organizerNotificationUpdatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"{eventArgs.UserName} has updated their comment on your event '{eventDetails.Name}'.",
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.CommentUpdated.ToString(),
                relatedEntityId: eventArgs.CommentId,
                details: organizerNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(organizerNotificationUpdatedEvent);

            var organizerNotificationDto = new NotificationDto
            {
                UserId = eventDetails.Organizer.Id,
                Message = organizerNotification.Message,
                CreatedAt = organizerNotification.CreatedAt,
                EventType = NotificationEventType.CommentUpdated,
                RelatedEntityId = eventArgs.CommentId,
                Details = organizerNotificationDetailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
            _logger.LogInformation("Broadcasted SignalR notification to the event organizer.");
        }
    }
}
