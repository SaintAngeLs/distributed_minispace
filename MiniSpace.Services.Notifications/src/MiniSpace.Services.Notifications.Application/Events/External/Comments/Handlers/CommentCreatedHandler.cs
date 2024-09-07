using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;
using Microsoft.AspNetCore.SignalR;

namespace MiniSpace.Services.Notifications.Application.Events.External.Comments.Handlers
{
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly ICommentsServiceClient _commentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<CommentCreatedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentCreatedHandler(
            ICommentsServiceClient commentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<CommentCreatedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _commentsServiceClient = commentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(CommentCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var commentDetails = await _commentsServiceClient.GetCommentAsync(@event.CommentId);
                if (commentDetails == null)
                {
                    _logger.LogError("No comment details found for CommentCreated event.");
                    return;
                }

                var entityOwnerId = Guid.Empty;
                string entityName = string.Empty;

                if (@event.CommentContext.Equals("OrganizationEvent", StringComparison.OrdinalIgnoreCase))
                {
                    var organizationEvent = await _eventsServiceClient.GetEventAsync(@event.ContextId);
                    if (organizationEvent != null)
                    {
                        entityOwnerId = organizationEvent.Organizer.Id;
                        entityName = organizationEvent.Name;
                    }
                }
                else if (@event.CommentContext.Equals("OrganizationPost", StringComparison.OrdinalIgnoreCase))
                {
                    var organizationPost = await _organizationsServiceClient.GetOrganizationPostAsync(@event.ContextId);
                    if (organizationPost != null)
                    {
                        entityOwnerId = organizationPost.Organization.OwnerId;
                        entityName = organizationPost.Title;
                    }
                }
                else if (@event.CommentContext.Equals("UserEvent", StringComparison.OrdinalIgnoreCase))
                {
                    var userEvent = await _eventsServiceClient.GetUserEventAsync(@event.ContextId);
                    if (userEvent != null)
                    {
                        entityOwnerId = userEvent.Organizer.Id;
                        entityName = userEvent.Name;
                    }
                }
                else if (@event.CommentContext.Equals("UserPost", StringComparison.OrdinalIgnoreCase))
                {
                    var userPost = await _commentsServiceClient.GetUserPostAsync(@event.ContextId);
                    if (userPost != null)
                    {
                        entityOwnerId = userPost.UserId;
                        entityName = userPost.Title;
                    }
                }
                else
                {
                    _logger.LogError($"Unknown CommentContext: {@event.CommentContext}");
                    return;
                }

                if (entityOwnerId == Guid.Empty)
                {
                    _logger.LogError("Entity owner not found for the context of the comment.");
                    return;
                }

                // Notify the entity owner (user or organization)
                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: entityOwnerId,
                    message: $"A new comment has been made on '{entityName}' by {commentDetails.StudentName}.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: @event.CommentId,
                    eventType: NotificationEventType.CommentCreated
                );

                var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(entityOwnerId)
                                    ?? new UserNotifications(entityOwnerId);

                userNotifications.AddNotification(notification);
                await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

                // Broadcast notification via SignalR
                var notificationDto = new NotificationDto
                {
                    UserId = entityOwnerId,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt,
                    EventType = NotificationEventType.CommentCreated,
                    RelatedEntityId = @event.CommentId,
                    Details = $"<p>A new comment was made on '{entityName}'.</p>"
                };

                await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to handle CommentCreated event: {ex.Message}");
            }
        }
    }
}
