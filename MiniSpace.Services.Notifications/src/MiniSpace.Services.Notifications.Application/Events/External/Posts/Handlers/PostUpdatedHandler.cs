using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Notifications.Application.Events.External.Posts.Handlers
{
    public class PostUpdatedHandler : IEventHandler<PostUpdated>
    {
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<PostUpdatedHandler> _logger;

        public PostUpdatedHandler(
            IFriendsServiceClient friendsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            IHubContext<NotificationHub> hubContext,
            ILogger<PostUpdatedHandler> logger)
        {
            _friendsServiceClient = friendsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(PostUpdated eventArgs, CancellationToken cancellationToken)
        {
            if (!eventArgs.ShouldNotify)
            {
                _logger.LogInformation("PostUpdated event received, but notifications are disabled.");
                return;
            }

            if (eventArgs.UserId.HasValue)
            {
                await NotifyFriendsAndFollowersAsync(eventArgs);
            }
            else if (eventArgs.OrganizationId.HasValue)
            {
                await NotifyOrganizationMembersAsync(eventArgs);
            }
        }

        private async Task NotifyFriendsAndFollowersAsync(PostUpdated eventArgs)
        {
            try
            {
                // Get the user's friends
                var friends = await _friendsServiceClient.GetFriendsAsync(eventArgs.UserId.Value);
                var friendIds = friends.Select(f => f.FriendId).ToList();

                // Get the user's followers
                var followers = await _friendsServiceClient.GetRequestsAsync(eventArgs.UserId.Value);
                var followerIds = followers.Select(f => f.InviterId).ToList();

                // Combine friends and followers into one list of user IDs to notify
                var userIdsToNotify = friendIds.Concat(followerIds).Distinct().ToList();

                foreach (var userId in userIdsToNotify)
                {
                    var notificationMessage = $"An updated post has been created by your friend or someone you follow (Post ID: {eventArgs.PostId}).";
                    await CreateAndSendNotification(userId, eventArgs, notificationMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify user's friends and followers: {ex.Message}");
            }
        }

        private async Task NotifyOrganizationMembersAsync(PostUpdated eventArgs)
        {
            try
            {
                var organizationMembers = await _organizationsServiceClient.GetOrganizationMembersAsync(eventArgs.OrganizationId.Value);

                if (organizationMembers != null && organizationMembers.Users != null)
                {
                    foreach (var member in organizationMembers.Users)
                    {
                        var notificationMessage = $"An updated post has been created by your organization (Post ID: {eventArgs.PostId}).";
                        await CreateAndSendNotification(member.Id, eventArgs, notificationMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify organization members: {ex.Message}");
            }
        }

        private async Task CreateAndSendNotification(Guid userId, PostUpdated eventArgs, string notificationMessage)
        {
            var detailsHtml = $"<p>{notificationMessage} Check out the updated post details here: {eventArgs.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: userId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.PostId,
                eventType: NotificationEventType.PostUpdated,
                details: detailsHtml
            );

            _logger.LogInformation($"Creating post update notification for user: {userId}");

            var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(userId);
            if (userNotifications == null)
            {
                userNotifications = new UserNotifications(userId);
            }

            userNotifications.AddNotification(notification);
            await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

            var notificationDto = new NotificationDto
            {
                UserId = userId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.PostUpdated,
                RelatedEntityId = eventArgs.PostId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to user with ID {userId}.");
        }
    }
}
