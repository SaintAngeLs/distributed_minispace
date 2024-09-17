using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Friends.Handlers
{
    public class FriendInvitedHandler : IEventHandler<FriendInvited>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<FriendInvitedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FriendInvitedHandler(
            INotificationRepository notificationRepository,
            IUserNotificationsRepository studentNotificationsRepository,  
            IStudentsServiceClient studentsServiceClient, 
            IEventMapper eventMapper, 
            IMessageBroker messageBroker,
            ILogger<FriendInvitedHandler> logger,
            IHubContext<NotificationHub> hubContext
        )
        {
            _notificationRepository = notificationRepository;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(FriendInvited @event, CancellationToken cancellationToken)
        {
            var inviter = await _studentsServiceClient.GetAsync(@event.InviterId);
            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";
            var detailsHtml = $"<p>View <a href='https://minispace.itsharppro.com/student-details/{@event.InviterId}'>{inviter.FirstName} {inviter.LastName}</a>'s profile to respond to the friend invitation.</p>";

            var notificationId = Guid.NewGuid();
            var notification = new Notification(
                notificationId: notificationId,
                userId: @event.InviteeId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.InviterId,
                eventType: NotificationEventType.NewFriendRequest,
                details: detailsHtml
            );

            await _notificationRepository.AddAsync(notification);

              

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(@event.InviteeId) ?? new UserNotifications(@event.InviteeId);
            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId,
                @event.InviteeId,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.NewFriendRequest.ToString(),
                @event.InviterId,
                detailsHtml
            );
            
            var notificationDto = new NotificationDto
            {
                UserId = @event.InviteeId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.FriendRequestAccepted,
                RelatedEntityId = @event.InviterId,
                Details = detailsHtml
            };

              await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Sent SignalR notification to all users with user id UserId={@event.InviteeId}.");

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
