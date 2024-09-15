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
                var entityOwnerId = Guid.Empty;
                string entityName = string.Empty;

                if (@event.CommentContext.Equals("OrganizationEvent", StringComparison.OrdinalIgnoreCase))
                {
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
                    var organizationPost = await _postsServiceClient.GetPostAsync(@event.ContextId);
                    if (organizationPost != null)
                    {
                        entityOwnerId = organizationPost.UserId.HasValue ? organizationPost.UserId.Value : Guid.Empty;
                        entityName = organizationPost.TextContent;
                    }
                }
                else if (@event.CommentContext.Equals("UserEvent", StringComparison.OrdinalIgnoreCase))
                {
                    var userEvent = await _eventsServiceClient.GetEventAsync(@event.ContextId);
                    if (userEvent != null)
                    {
                        entityOwnerId = userEvent.Organizer.Id;
                        entityName = userEvent.Name;
                    }
                }
                else if (@event.CommentContext.Equals("UserPost", StringComparison.OrdinalIgnoreCase))
                {
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

                var notificationMessage = $"A new comment has been made on '{entityName}' by {@event.UserName}. <img src='{@event.ProfileImageUrl}' alt='Profile Image' style='width:50px;height:50px;' />";
                
                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: entityOwnerId,
                    message: notificationMessage,
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

                var notificationDto = new NotificationDto
                {
                    UserId = entityOwnerId,
                    Message = notificationMessage,
                    CreatedAt = notification.CreatedAt,
                    EventType = NotificationEventType.CommentCreated,
                    RelatedEntityId = @event.CommentId,
                    Details = $"<p>{@event.UserName} commented: '{@event.TextContent}' on '{entityName}'.</p><img src='{@event.ProfileImageUrl}' alt='Profile Image' style='width:50px;height:50px;' />"
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
