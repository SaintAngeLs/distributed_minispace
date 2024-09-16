using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Text.Json;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IUserNotificationsRepository _userNotificationsRepository;
        private readonly ILogger<EventCreatedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventCreatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IFriendsServiceClient friendsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IUserNotificationsRepository userNotificationsRepository,
            ILogger<EventCreatedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _friendsServiceClient = friendsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _userNotificationsRepository = userNotificationsRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventCreated eventCreated, CancellationToken cancellationToken)
        {
            try
            {
                // Check if the event is organized by an organization or a user
                if (eventCreated.OrganizerType.Equals("Organization", StringComparison.OrdinalIgnoreCase))
                {
                    // Notify all members of the organization
                    await NotifyOrganizationMembersAsync(eventCreated);
                }
                else if (eventCreated.OrganizerType.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    // Notify user's friends and followers
                    await NotifyUserFriendsAndFollowersAsync(eventCreated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to handle EventCreated event: {ex.Message}");
            }
        }

        private async Task NotifyOrganizationMembersAsync(EventCreated eventCreated)
        {
            try
            {
                var organizationUsers = await _organizationsServiceClient.GetOrganizationMembersAsync(eventCreated.OrganizerId);

                if (organizationUsers != null && organizationUsers.Users != null)
                {
                    foreach (var user in organizationUsers.Users)
                    {
                        var notificationMessage = $"A new event has been created in your organization (Event ID: {eventCreated.EventId}).";

                        var notification = new Notification(
                            notificationId: Guid.NewGuid(),
                            userId: user.Id, // Corrected from 'Members' to 'Users'
                            message: notificationMessage,
                            status: NotificationStatus.Unread,
                            createdAt: DateTime.UtcNow,
                            updatedAt: null,
                            relatedEntityId: eventCreated.EventId,
                            eventType: NotificationEventType.NewEvent
                        );

                        var userNotifications = await _userNotificationsRepository.GetByUserIdAsync(user.Id)
                                              ?? new UserNotifications(user.Id);

                        userNotifications.AddNotification(notification);
                        await _userNotificationsRepository.AddOrUpdateAsync(userNotifications);

                        // Broadcast the notification to the user using SignalR
                        var notificationDto = new NotificationDto
                        {
                            UserId = user.Id,
                            Message = notificationMessage,
                            CreatedAt = notification.CreatedAt,
                            EventType = NotificationEventType.NewEvent,
                            RelatedEntityId = eventCreated.EventId,
                            Details = $"<p>A new event has been created in your organization.</p>"
                        };

                        await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify organization members: {ex.Message}");
            }
        }

        private async Task NotifyUserFriendsAndFollowersAsync(EventCreated eventCreated)
        {
            try
            {
                // Get the user's friends
                var friends = await _friendsServiceClient.GetFriendsAsync(eventCreated.OrganizerId);
                var friendIds = friends.Select(f => f.FriendId).ToList();

                // Get the users who have requested friendship (followers)
                var followers = await _friendsServiceClient.GetRequestsAsync(eventCreated.OrganizerId);
                var followerIds = followers.Select(f => f.InviterId).ToList(); // Corrected from 'RequesterId' to 'InviterId'

                // Combine friends and followers into one list of user IDs to notify
                var userIdsToNotify = friendIds.Concat(followerIds).Distinct().ToList();

                foreach (var userId in userIdsToNotify)
                {
                    var notificationMessage = $"A new event has been created by your friend or someone you follow (Event ID: {eventCreated.EventId}).";

                    var notification = new Notification(
                        notificationId: Guid.NewGuid(),
                        userId: userId,
                        message: notificationMessage,
                        status: NotificationStatus.Unread,
                        createdAt: DateTime.UtcNow,
                        updatedAt: null,
                        relatedEntityId: eventCreated.EventId,
                        eventType: NotificationEventType.NewEvent
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
                        EventType = NotificationEventType.NewEvent,
                        RelatedEntityId = eventCreated.EventId,
                        Details = $"<p>A new event has been created by a user you follow or are friends with.</p>"
                    };

                    await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to notify user's friends and followers: {ex.Message}");
            }
        }
    }
}
