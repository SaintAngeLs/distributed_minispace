using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly ICommentsServiceClient _commentsServiceClient;
        private readonly ILogger<CommentCreatedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentCreatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository,
            ICommentsServiceClient commentsServiceClient,
            ILogger<CommentCreatedHandler> logger,
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

        public async Task HandleAsync(CommentCreated eventArgs, CancellationToken cancellationToken)
        {
            CommentDto commentDetails;
            try
            {
                commentDetails = await _commentsServiceClient.GetCommentAsync(eventArgs.CommentId);
                if (commentDetails == null)
                {
                    _logger.LogError("No comment details found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve comment details: {ex.Message}");
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

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(commentDetails.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(commentDetails.StudentId);
            }

            var userNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: commentDetails.StudentId,
                message: $"Thank you for your comment on the event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentCreated
            );

            studentNotifications.AddNotification(userNotification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var userNotificationDetailsHtml = $"<p>Your comment on the event '{eventDetails.Name}' has been posted successfully.</p>";

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: commentDetails.StudentId,
                message: $"Thank you for your comment on the event '{eventDetails.Name}'.",
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.CommentCreated.ToString(),
                relatedEntityId: eventArgs.CommentId,
                details: userNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = commentDetails.StudentId,
                Message = $"Thank you for your comment on the event '{eventDetails.Name}'.",
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.CommentCreated,
                RelatedEntityId = eventArgs.CommentId,
                Details = userNotificationDetailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation("Broadcasted SignalR notification to all users.");

            var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventDetails.Organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new StudentNotifications(eventDetails.Organizer.Id);
            }

            var organizerNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"A new comment has been posted by {commentDetails.StudentName} on your event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentCreated
            );

            organizerNotifications.AddNotification(organizerNotification);
            await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

            var organizerNotificationDetailsHtml = $"<p>{commentDetails.StudentName} commented on your event '{eventDetails.Name}': {commentDetails.CommentContext}</p>";

            var organizerNotificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"A new comment has been posted by {commentDetails.StudentName} on your event '{eventDetails.Name}'.",
                createdAt: organizerNotification.CreatedAt,
                eventType: NotificationEventType.CommentCreated.ToString(),
                relatedEntityId: eventArgs.CommentId,
                details: organizerNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);

            var organizerNotificationDto = new NotificationDto
            {
                UserId = eventDetails.Organizer.Id,
                Message = $"A new comment has been posted by {commentDetails.StudentName} on your event '{eventDetails.Name}'.",
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.CommentCreated,
                RelatedEntityId = eventArgs.CommentId,
                Details = organizerNotificationDetailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
            _logger.LogInformation("Broadcasted SignalR notification to all users.");
        }
    }
}
