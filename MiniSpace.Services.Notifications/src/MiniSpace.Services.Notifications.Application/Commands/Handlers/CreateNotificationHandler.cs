using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Events.External;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class CreateNotificationHandler : ICommandHandler<CreateNotification>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IBaseUrlService _baseUrlService;

        public CreateNotificationHandler(
            IStudentNotificationsRepository studentNotificationsRepository,
            IFriendsServiceClient friendsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient,
            IOrganizationsServiceClient organizationsServiceClient,
            IMessageBroker messageBroker,
            IBaseUrlService baseUrlService
        )
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _friendsServiceClient = friendsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
            _organizationsServiceClient = organizationsServiceClient;
            _messageBroker = messageBroker;
            _baseUrlService = baseUrlService;
        }

        public async Task HandleAsync(CreateNotification command, CancellationToken cancellationToken = default)
        {
            string notificationMessage = string.Empty;
            string detailedMessage = string.Empty;
            string notificationLink = string.Empty;

            var baseUrl = _baseUrlService.GetBaseUrl();

            switch (command.NotificationType.ToLower())
            {
                case "eventinvitation":
                    if (command.EventId.HasValue)
                    {
                        var eventDetails = await _eventsServiceClient.GetEventAsync(command.EventId.Value);
                        notificationLink = $"{baseUrl}/events/{eventDetails.Id}";

                        notificationMessage = $"<p>You have been invited to the event '{eventDetails.Name}'. <a href='{notificationLink}'>Click here</a> to learn more.</p>";
                        detailedMessage = $"<p>Event: {eventDetails.Name} organized by {eventDetails.Organizer.Name}. " +
                            $"The event will take place from {eventDetails.StartDate:yyyy-MM-dd} to {eventDetails.EndDate:yyyy-MM-dd} at {eventDetails.Location.Street}, {eventDetails.Location.City}. " +
                            $"Learn more: <a href='{notificationLink}'>here</a>.</p>";
                    }
                    break;

                case "postcomment":
                    if (command.PostId.HasValue)
                    {
                        var postDetails = await _postsServiceClient.GetPostAsync(command.PostId.Value);
                        notificationLink = $"{baseUrl}/posts/{postDetails.Id}";

                        notificationMessage = $"<p>New comment on your post '{postDetails.TextContent}'. <a href='{notificationLink}'>Click here</a> to view.</p>";
                        detailedMessage = $"<p>Your post '{postDetails.TextContent}' has received a new comment. " +
                            $"Learn more: <a href='{notificationLink}'>here</a>.</p>";
                    }
                    break;

                case "organizationupdate":
                    if (command.OrganizationId.HasValue)
                    {
                        var orgDetails = await _organizationsServiceClient.GetOrganizationAsync(command.OrganizationId.Value);
                        notificationLink = $"{baseUrl}/organizations/{orgDetails.Id}";

                        notificationMessage = $"<p>Update from your organization '{orgDetails.Name}'. <a href='{notificationLink}'>Click here</a> to read more.</p>";
                        detailedMessage = $"<p>Your organization '{orgDetails.Name}' has shared a new update. " +
                            $"Learn more: <a href='{notificationLink}'>here</a>.</p>";
                    }
                    break;

                case "generalnotification":
                default:
                    notificationMessage = command.Message;
                    detailedMessage = command.Message;
                    break;
            }

            // Send notifications to all users
            foreach (var userId in command.UsersId)
            {
                var notification = new Notification(
                    command.NotificationId,
                    userId,
                    detailedMessage,
                    NotificationStatus.Unread,
                    DateTime.UtcNow,
                    null,
                    NotificationEventType.NewEventInvitaion,
                    command.EventId ?? Guid.Empty,
                    null
                );

                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(userId);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(userId);
                }

                studentNotifications.AddNotification(notification);
                await _studentNotificationsRepository.UpdateAsync(studentNotifications);

                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: notification.NotificationId,
                    userId: userId,
                    message: notificationMessage,
                    createdAt: notification.CreatedAt,
                    eventType: command.NotificationType,
                    relatedEntityId: command.EventId ?? command.PostId ?? command.OrganizationId ?? Guid.Empty,
                    details: detailedMessage
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }
        }
    }
}
