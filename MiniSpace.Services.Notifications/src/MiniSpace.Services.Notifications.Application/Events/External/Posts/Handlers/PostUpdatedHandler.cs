using System;
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
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Dto.Events;
using MiniSpace.Services.Notifications.Application.Dto.Posts;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class PostUpdatedHandler : IEventHandler<PostUpdated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserNotificationsRepository _studentNotificationsRepository;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<PostUpdatedHandler> _logger;

        public PostUpdatedHandler(
            IMessageBroker messageBroker,
            IUserNotificationsRepository studentNotificationsRepository,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient,
            IStudentsServiceClient studentsServiceClient,
            IHubContext<NotificationHub> hubContext,
            ILogger<PostUpdatedHandler> logger)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
            _studentsServiceClient = studentsServiceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(PostUpdated eventArgs, CancellationToken cancellationToken)
        {
            var post = await _postsServiceClient.GetPostAsync(eventArgs.PostId);
            if (post == null)
            {
                _logger.LogError("Post not found.");
                return;
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(post.EventId);
            if (eventDetails == null)
            {
                _logger.LogError("Event not found for the post.");
                return;
            }

            var eventParticipants = await _eventsServiceClient.GetParticipantsAsync(post.EventId);
            if (eventParticipants == null)
            {
                _logger.LogError("No participants found for the event.");
                return;
            }

            foreach (var studentParticipant in eventParticipants.InterestedStudents)
            {
                var student = await _studentsServiceClient.GetAsync(studentParticipant.StudentId);
                if (student != null)
                {
                    await NotifyStudent(student, eventDetails, post);
                }
            }

            foreach (var studentParticipant in eventParticipants.SignedUpStudents)
            {
                var student = await _studentsServiceClient.GetAsync(studentParticipant.StudentId);
                if (student != null)
                {
                    await NotifyStudent(student, eventDetails, post);
                }
            }

            // Notify the organizer
            await NotifyOrganizer(eventDetails.Organizer, eventDetails, post);
        }

        private async Task NotifyStudent(UserDto student, EventDto eventDetails, PostDto post)
        {
            var notificationMessage = $"An updated post is available for an event you are interested in: '{eventDetails.Name}'.";
            var detailsHtml = $"<p>{notificationMessage} Check out the updated post details here: {post.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: student.Id,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: post.EventId ?? post.OrganizationId ?? post.UserId ?? post.Id,
                eventType: NotificationEventType.PostUpdated,
                details: detailsHtml
            );

            _logger.LogInformation($"Creating post update notification for user: {student.Id}");

            var studentNotifications = await _studentNotificationsRepository.GetByUserIdAsync(student.Id);
            if (studentNotifications == null)
            {
                studentNotifications = new UserNotifications(student.Id);
            }

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                student.Id,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.PostUpdated.ToString(),
                relatedEntityId: post.EventId ?? post.OrganizationId ?? post.UserId ?? post.Id,
                detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = student.Id,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.PostUpdated,
                RelatedEntityId = post.EventId,
                Details = detailsHtml
            };

            // Broadcast SignalR notification
            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to student with ID {student.Id}.");
        }

        private async Task NotifyOrganizer(OrganizerDto organizer, EventDto eventDetails, PostDto post)
        {
            var notificationMessage = $"An updated post is available for your event '{eventDetails.Name}'.";
            var detailsHtml = $"<p>{notificationMessage} View the updated post here: {post.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: organizer.Id,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: post.EventId ?? post.OrganizationId ?? post.UserId ?? post.Id,
                eventType: NotificationEventType.PostUpdated,
                details: detailsHtml
            );

            _logger.LogInformation($"Creating post update notification for organizer: {organizer.Id}");

            var organizerNotifications = await _studentNotificationsRepository.GetByUserIdAsync(organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new UserNotifications(organizer.Id);
            }

            organizerNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(organizerNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notification.NotificationId,
                organizer.Id,
                notificationMessage,
                DateTime.UtcNow,
                NotificationEventType.PostUpdated.ToString(),
                post.EventId ?? post.OrganizationId ?? post.UserId ?? post.Id,
                detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var notificationDto = new NotificationDto
            {
                UserId = organizer.Id,
                Message = notificationMessage,
                CreatedAt = DateTime.UtcNow,
                EventType = NotificationEventType.PostUpdated,
                RelatedEntityId = post.EventId ?? post.OrganizationId ?? post.UserId ?? post.Id,
                Details = detailsHtml
            };

            // Broadcast SignalR notification
            await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
            _logger.LogInformation($"Broadcasted SignalR notification to organizer with ID {organizer.Id}.");
        }
    }
}
