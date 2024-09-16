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

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<EventDeletedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventDeletedHandler(
            IFriendsServiceClient friendsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<EventDeletedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _friendsServiceClient = friendsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            try
            {
                if (eventDeleted.OrganizerType.Equals("Organization", StringComparison.OrdinalIgnoreCase))
                {
                    await NotifyOrganizationMembersAsync(eventDeleted);
                }
                else if (eventDeleted.OrganizerType.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    await NotifyUserFriendsAndFollowersAsync(eventDeleted);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to handle EventDeleted event: {ex.Message}");
            }
        }

        private async Task NotifyOrganizationMembersAsync(EventDeleted eventDeleted)
        {
            try
            {
                var organizationUsers = await _organizationsServiceClient.GetOrganizationMembersAsync(eventDeleted.OrganizerId);

                if (organizationUsers != null && organizationUsers.Users != null)
                {
                    foreach (var user in organizationUsers.Users)
                    {
                        var notificationMessage = $"The event '{eventDeleted.EventName}' (Event ID: {eventDeleted.EventId}) scheduled from {eventDeleted.StartDate:yyyy-MM-dd} to {eventDeleted.EndDate:yyyy-MM-dd} has been cancelled.";

                        var notification = new Notification(
                            notificationId: Guid.NewGuid(),
                            userId: user.Id,
                            message: notificationMessage,
                            status: NotificationStatus.Unread,
                            createdAt: DateTime.UtcNow,
                            updatedAt: null,
                            relatedEntityId: eventDeleted.EventId,
                            eventType: NotificationEventType.EventDeleted
                        );

                        var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(user.Id)
                                              ?? new UserNotifications(user.Id);

                        userNotifications.AddNotification(notification);
                        await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

                        var notificationDto = new NotificationDto
                        {
                            UserId = user.Id,
                            Message = notificationMessage,
                            CreatedAt = notification.CreatedAt,
                            EventType = NotificationEventType.EventDeleted,
                            RelatedEntityId = eventDeleted.EventId,
                            Details = $"<p>The event '{eventDeleted.EventName}' has been cancelled by the organization.</p>"
                        };

                        await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify organization members about the deleted event: {ex.Message}");
            }
        }

        private async Task NotifyUserFriendsAndFollowersAsync(EventDeleted eventDeleted)
        {
            try
            {
                var friends = await _friendsServiceClient.GetFriendsAsync(eventDeleted.OrganizerId);
                var friendIds = friends.Select(f => f.FriendId).ToList();

                var followers = await _friendsServiceClient.GetRequestsAsync(eventDeleted.OrganizerId);
                var followerIds = followers.Select(f => f.InviterId).ToList();

                var userIdsToNotify = friendIds.Concat(followerIds).Distinct().ToList();

                foreach (var userId in userIdsToNotify)
                {
                    var notificationMessage = $"An event '{eventDeleted.EventName}' (Event ID: {eventDeleted.EventId}) scheduled from {eventDeleted.StartDate:yyyy-MM-dd} to {eventDeleted.EndDate:yyyy-MM-dd} created by your friend or someone you follow has been cancelled.";

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

                    var notificationDto = new NotificationDto
                    {
                        UserId = userId,
                        Message = notificationMessage,
                        CreatedAt = notification.CreatedAt,
                        EventType = NotificationEventType.EventDeleted,
                        RelatedEntityId = eventDeleted.EventId,
                        Details = $"<p>The event '{eventDeleted.EventName}' has been cancelled by the organizer.</p>"
                    };

                    await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify friends and followers about the deleted event: {ex.Message}");
            }
        }
    }
}
