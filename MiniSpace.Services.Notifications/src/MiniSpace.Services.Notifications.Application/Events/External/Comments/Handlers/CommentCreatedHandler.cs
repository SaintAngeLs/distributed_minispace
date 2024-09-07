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
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Events.External.Comments.Handlers
{
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly ICommentsServiceClient _commentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<CommentCreatedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentCreatedHandler(
            ICommentsServiceClient commentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<CommentCreatedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _commentsServiceClient = commentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(CommentCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                // Fetch comment details from the comment service
                var commentDetails = await _commentsServiceClient.GetCommentAsync(@event.CommentId);
                if (commentDetails == null)
                {
                    _logger.LogError("No comment details found for CommentCreated event.");
                    return;
                }

                // Initialize entity owner and name variables
                var entityOwnerId = Guid.Empty;
                string entityName = string.Empty;

                // Check if it's an event or post
                if (@event.CommentContext.Equals("OrganizationEvent", StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch organization event details
                    var organizationEvent = await _eventsServiceClient.GetEventAsync(@event.ContextId);
                    if (organizationEvent != null)
                    {
                        entityOwnerId = organizationEvent.Organizer.OrganizationId != Guid.Empty 
                            ? organizationEvent.Organizer.OrganizationId 
                            : organizationEvent.Organizer.Id;
                        entityName = organizationEvent.Name;
                    }
                }
                else if (@event.CommentContext.Equals("OrganizationPost", StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch organization post details
                    var organizationPost = await  _postsServiceClient.GetPostAsync(@event.ContextId);
                    if (organizationPost != null)
                    {
                        entityOwnerId = organizationPost.UserId.HasValue ? organizationPost.UserId.Value : Guid.Empty;
                        entityName = organizationPost.TextContent;
                    }
                }
                else if (@event.CommentContext.Equals("UserEvent", StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch user event details
                    var userEvent = await _eventsServiceClient.GetEventAsync(@event.ContextId);
                    if (userEvent != null)
                    {
                        entityOwnerId = userEvent.Organizer.Id;
                        entityName = userEvent.Name;
                    }
                }
                else if (@event.CommentContext.Equals("UserPost", StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch user post details
                    var userPost = await _postsServiceClient.GetPostAsync(@event.ContextId);
                    if (userPost != null)
                    {
                        entityOwnerId = userPost.UserId.HasValue ? userPost.UserId.Value : Guid.Empty;
                        entityName = userPost.TextContent;
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
