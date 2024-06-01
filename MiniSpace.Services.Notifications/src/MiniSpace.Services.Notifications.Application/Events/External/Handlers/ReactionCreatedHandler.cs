using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class ReactionCreatedHandler : IEventHandler<ReactionCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IReactionsServiceClient _reactionsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;

         public ReactionCreatedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IReactionsServiceClient reactionsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _reactionsServiceClient = reactionsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
        }

        public async Task HandleAsync(ReactionCreated eventArgs, CancellationToken cancellationToken)
        {
            var reactionDetails = await _reactionsServiceClient.GetReactionsAsync();
            var reaction = reactionDetails.FirstOrDefault(r => r.Id == eventArgs.ReactionId);

            if (reaction == null)
            {
                Console.WriteLine("Reaction details not found.");
                return;
            }

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(reaction.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(reaction.StudentId);
            }

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: reaction.StudentId,
                message: $"Your reaction has been recorded.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: reaction.ContentId,
                eventType: NotificationEventType.ReactionAdded
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

            var notificationDetailsHtml = $@"
                <p>Thank you for your reaction! Your interaction helps us to better understand what content resonates with our community.</p>";

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: reaction.StudentId,
                message: $"Your reaction has been recorded.",
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.ReactionAdded.ToString(),
                relatedEntityId: reaction.ContentId,
                details: notificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

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

                var organizerNotification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: organizerId.Value,
                    message: $"A new reaction has been added to your content.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: reaction.ContentId,
                    eventType: NotificationEventType.ReactionAdded
                );

                organizerNotifications.AddNotification(organizerNotification);
                await _studentNotificationsRepository.UpdateAsync(organizerNotifications);

                await _messageBroker.PublishAsync(new NotificationCreated(
                    notificationId: Guid.NewGuid(),
                    userId: organizerId.Value,
                    message: $"A new reaction has been added to your content.",
                    createdAt: DateTime.UtcNow,
                    eventType: NotificationEventType.ReactionAdded.ToString(),
                    relatedEntityId: reaction.ContentId,
                    details: $"<p>{reaction.StudentFullName} reacted to your content.</p>"
                ));
            }
        }
    }
}
