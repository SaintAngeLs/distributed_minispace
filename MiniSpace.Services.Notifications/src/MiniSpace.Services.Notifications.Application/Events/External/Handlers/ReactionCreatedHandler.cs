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
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReactionCreatedHandler : IEventHandler<ReactionCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IReactionsServiceClient _reactionsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<ReactionCreatedHandler> _logger;

        public ReactionCreatedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IReactionsServiceClient reactionsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<ReactionCreatedHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _reactionsServiceClient = reactionsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(ReactionCreated eventArgs, CancellationToken cancellationToken)
        {
            var reactionDetails = await _reactionsServiceClient.GetReactionsAsync();
            var reaction = reactionDetails.FirstOrDefault(r => r.Id == eventArgs.ReactionId);

            if (reaction == null)
            {
                _logger.LogError("Reaction details not found.");
                return;
            }

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(reaction.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(reaction.StudentId);
            }

            var notificationMessage = "Your reaction has been recorded.";
            var notificationDetailsHtml = "<p>Thank you for your reaction! Your interaction helps us to better understand what content resonates with our community.</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: reaction.StudentId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: reaction.ContentId,
                eventType: NotificationEventType.ReactionAdded,
                details: notificationDetailsHtml
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                reaction.StudentId,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.ReactionAdded.ToString(),
                reaction.ContentId,
                notificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = reaction.StudentId,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.ReactionAdded,
                RelatedEntityId = reaction.ContentId,
                Details = notificationDetailsHtml
            };

            // Broadcast SignalR notification to the student
            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to student with ID {reaction.StudentId}.");

            // Notify the organizer
            Guid? organizerId = null;
            if (reaction.ContentType == ReactionContentType.Event)
            {
                var eventDetails = await _eventsServiceClient.GetEventAsync(reaction.ContentId);
                organizerId = eventDetails?.Organizer.Id;
            }
            else if (reaction.ContentType == ReactionContentType.Post)
            {
                var postDetails = await _postsServiceClient.GetPostAsync(reaction.ContentId);
                organizerId = postDetails?.OrganizerId;
            }

            if (organizerId.HasValue)
            {
                var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(organizerId.Value);
                if (organizerNotifications == null)
                {
                    organizerNotifications = new StudentNotifications(organizerId.Value);
                }

                var organizerNotificationMessage = "A new reaction has been added to your content.";
                var organizerNotificationDetailsHtml = $"<p>{reaction.StudentFullName} reacted to your content.</p>";

                var organizerNotification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: organizerId.Value,
                    message: organizerNotificationMessage,
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: reaction.ContentId,
                    eventType: NotificationEventType.ReactionAdded,
                    details: organizerNotificationDetailsHtml
                );

                organizerNotifications.AddNotification(organizerNotification);
                await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

                var organizerNotificationCreatedEvent = new NotificationCreated(
                    organizerNotification.NotificationId,
                    organizerId.Value,
                    organizerNotificationMessage,
                    DateTime.UtcNow,
                    NotificationEventType.ReactionAdded.ToString(),
                    reaction.ContentId,
                    organizerNotificationDetailsHtml
                );

                await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);

                var organizerNotificationDto = new NotificationDto
                {
                    UserId = organizerId.Value,
                    Message = organizerNotificationMessage,
                    CreatedAt = DateTime.UtcNow,
                    EventType = NotificationEventType.ReactionAdded,
                    RelatedEntityId = reaction.ContentId,
                    Details = organizerNotificationDetailsHtml
                };

                // Broadcast SignalR notification to the organizer
                await NotificationHub.BroadcastNotification(_hubContext, organizerNotificationDto, _logger);
                _logger.LogInformation($"Broadcasted SignalR notification to organizer with ID {organizerId.Value}.");
            }
        }
    }
}
