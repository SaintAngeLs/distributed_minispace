using System;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Events.External.Comments.Handlers
{
    public class LikeAddedHandler : IEventHandler<LikeAdded>
    {
        private readonly ICommentsServiceClient _commentsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<LikeAddedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public LikeAddedHandler(
            ICommentsServiceClient commentsServiceClient,
            IPostsServiceClient postsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<LikeAddedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _commentsServiceClient = commentsServiceClient;
            _postsServiceClient = postsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(LikeAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var commentDetails = await _commentsServiceClient.GetCommentAsync(@event.CommentId);
                if (commentDetails == null)
                {
                    _logger.LogError($"No comment details found for LikeAdded event. CommentId: {@event.CommentId}");
                    return;
                }

                var entityOwnerId = commentDetails.UserId;
                var entityName = commentDetails.TextContent;

                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: entityOwnerId,
                    message: $"{@event.UserName} liked your comment '{entityName}'.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: @event.CommentId,
                    eventType: NotificationEventType.CommentLikeAdded
                );

                var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(entityOwnerId)
                                    ?? new UserNotifications(entityOwnerId);

                userNotifications.AddNotification(notification);
                await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

                var notificationDetails = $"<p>{@event.UserName} liked your comment: '{@event.CommentContext}'.</p>" +
                                          $"<img src='{@event.ProfileImageUrl}' alt='Profile Image' style='width:50px;height:50px;' />";

                var notificationDto = new NotificationDto
                {
                    UserId = entityOwnerId,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt,
                    EventType = NotificationEventType.CommentLikeAdded,
                    RelatedEntityId = @event.CommentId,
                    Details = notificationDetails
                };

                await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to handle LikeAdded event: {ex.Message}");
            }
        }
    }
}
