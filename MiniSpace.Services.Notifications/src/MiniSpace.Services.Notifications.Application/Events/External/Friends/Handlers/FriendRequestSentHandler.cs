using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendRequestSentHandler : IEventHandler<FriendRequestSent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<FriendRequestSentHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FriendRequestSentHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            ILogger<FriendRequestSentHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(FriendRequestSent @event, CancellationToken cancellationToken)
        {
            var inviter = await _studentsServiceClient.GetAsync(@event.InviterId);
            if (inviter == null)
            {
                _logger.LogError($"Inviter not found with ID={@event.InviterId}");
                return;
            }

            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";
            var detailsHtml = $"<p>View <a href='https://minispace.itsharppro.com/student-details/{@event.InviterId}'>{inviter.FirstName} {inviter.LastName}</a>'s profile to respond to the friend invitation.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: @event.InviterId,
                eventType: NotificationEventType.NewFriendRequest,
                details: detailsHtml
            );

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(@event.InviteeId) ?? new UserNotifications(@event.InviteeId);
            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
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
            _logger.LogInformation("Sent SignalR notification to all users.");

            // await NotificationHub.SendNotification(_hubContext, @event.InviteeId.ToString(), notificationDto, _logger);
            // _logger.LogInformation($"Sent SignalR notification to UserId={@event.InviteeId}");

            await _messageBroker.PublishAsync(notificationCreatedEvent);
            _logger.LogInformation($"Published NotificationCreated event for UserId={notification.UserId}");
        }
    }
}
