using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Reactions.Handlers
{
    public class ReactionCreatedHandler : IEventHandler<ReactionCreated>
    {
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<ReactionCreatedHandler> _logger;

        public ReactionCreatedHandler(
            IFriendsServiceClient friendsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IPostsServiceClient postsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            IHubContext<NotificationHub> hubContext,
            ILogger<ReactionCreatedHandler> logger)
        {
            _friendsServiceClient = friendsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _postsServiceClient = postsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(ReactionCreated eventArgs, CancellationToken cancellationToken)
        {
            if (eventArgs.ContentType == "Post" || eventArgs.ContentType == "Event")
            {
                // Notify the content creator first
                await NotifyContentCreatorAsync(eventArgs);

                if (eventArgs.TargetType == "User")
                {
                    await NotifyFriendsAndFollowersAsync(eventArgs);
                }
                else if (eventArgs.TargetType == "Organization")
                {
                    await NotifyOrganizationMembersAsync(eventArgs);
                }
            }
        }

        private async Task NotifyContentCreatorAsync(ReactionCreated eventArgs)
        {
            try
            {
                Guid? contentCreatorId = null;

                // Check the content type and find the content creator
                if (eventArgs.ContentType == "Post")
                {
                    var postDetails = await _postsServiceClient.GetPostAsync(eventArgs.ContentId);
                    contentCreatorId = postDetails?.UserId ?? postDetails?.OrganizationId;
                }
                else if (eventArgs.ContentType == "Event")
                {
                    var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.ContentId);
                    contentCreatorId = eventDetails?.Organizer.Id ?? eventDetails?.Organizer.OrganizationId;
                }

                if (contentCreatorId.HasValue)
                {
                    var notificationMessage = $"Your content has received a new reaction: {eventArgs.ReactionType}.";
                    await CreateAndSendNotification(contentCreatorId.Value, eventArgs, notificationMessage, isCreator: true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify content creator: {ex.Message}");
            }
        }

        private async Task NotifyFriendsAndFollowersAsync(ReactionCreated eventArgs)
        {
            try
            {
                var friends = await _friendsServiceClient.GetFriendsAsync(eventArgs.UserId);
                var friendIds = friends.Select(f => f.FriendId).ToList();

                var followers = await _friendsServiceClient.GetRequestsAsync(eventArgs.UserId);
                var followerIds = followers.Select(f => f.InviterId).ToList();

                var userIdsToNotify = friendIds.Concat(followerIds).Distinct().ToList();

                foreach (var userId in userIdsToNotify)
                {
                    var notificationMessage = $"A new reaction has been added by your friend or someone you follow (Content ID: {eventArgs.ContentId}).";
                    await CreateAndSendNotification(userId, eventArgs, notificationMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify user's friends and followers: {ex.Message}");
            }
        }

        private async Task NotifyOrganizationMembersAsync(ReactionCreated eventArgs)
        {
            try
            {
                var organizationMembers = await _organizationsServiceClient.GetOrganizationMembersAsync(eventArgs.UserId);

                if (organizationMembers != null && organizationMembers.Users != null)
                {
                    foreach (var member in organizationMembers.Users)
                    {
                        var notificationMessage = $"A new reaction has been added to content created by your organization (Content ID: {eventArgs.ContentId}).";
                        await CreateAndSendNotification(member.Id, eventArgs, notificationMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify organization members: {ex.Message}");
            }
        }

        private async Task CreateAndSendNotification(Guid userId, ReactionCreated eventArgs, string notificationMessage, bool isCreator = false)
        {
            var detailsHtml = $"<p>{notificationMessage} Reaction: {eventArgs.ReactionType} on content.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: userId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.ContentId,
                eventType: isCreator ? NotificationEventType.ReactionAdded : NotificationEventType.ReactionAdded,
                details: detailsHtml
            );

            _logger.LogInformation($"Creating reaction notification for user: {userId}");

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
                EventType = isCreator ? NotificationEventType.ReactionAdded : NotificationEventType.ReactionAdded,
                RelatedEntityId = eventArgs.ContentId,
                Details = detailsHtml
            };

            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to user with ID {userId}.");
        }
    }
}
