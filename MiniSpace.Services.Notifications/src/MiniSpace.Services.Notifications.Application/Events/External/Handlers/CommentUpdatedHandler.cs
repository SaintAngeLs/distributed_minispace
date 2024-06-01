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
    public class CommentUpdatedHandler : IEventHandler<CommentUpdated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly ICommentsServiceClient _commentsServiceClient;

        public CommentUpdatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository,
            ICommentsServiceClient commentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
            _commentsServiceClient = commentsServiceClient;
        }

        public async Task HandleAsync(CommentUpdated eventArgs, CancellationToken cancellationToken)
        {
            CommentDto commentDetails;
            try
            {
                commentDetails = await _commentsServiceClient.GetCommentAsync(eventArgs.CommentId);
                if (commentDetails == null)
                {
                    Console.WriteLine("Updated comment details not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve updated comment details: {ex.Message}");
                throw;
            }

            EventDto eventDetails;
            try
            {
                eventDetails = await _eventsServiceClient.GetEventAsync(commentDetails.ContextId);
                if (eventDetails == null)
                {
                    Console.WriteLine("Event details for comment context not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve event details for comment context: {ex.Message}");
                throw;
            }

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(commentDetails.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(commentDetails.StudentId);
            }

            var userNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: commentDetails.StudentId,
                message: $"Your updated comment on the event '{eventDetails.Name}' has been processed.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow, 
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentUpdated
            );

            studentNotifications.AddNotification(userNotification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

            var userNotificationDetailsHtml = $"<p>Your updated comment on the event '{eventDetails.Name}' is now visible.</p>";

            var notificationUpdatedEvent = new NotificationCreated(
                notificationId: userNotification.NotificationId,
                userId: userNotification.UserId,
                message: userNotification.Message,
                createdAt: userNotification.CreatedAt,
                eventType: "CommentUpdated",
                relatedEntityId: userNotification.RelatedEntityId,
                details: userNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationUpdatedEvent);

            var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventDetails.Organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new StudentNotifications(eventDetails.Organizer.Id);
            }

            var organizerNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"{commentDetails.StudentName} has updated their comment on your event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow, 
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentUpdated
            );

            organizerNotifications.AddNotification(organizerNotification);
            await _studentNotificationsRepository.UpdateAsync(organizerNotifications);

            var organizerNotificationDetailsHtml = $"<p>{commentDetails.StudentName} updated their comment on your event '{eventDetails.Name}': {commentDetails.CommentContext}</p>";

            var organizerNotificationUpdatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"{commentDetails.StudentName} has updated their comment on your event '{eventDetails.Name}'.",
                createdAt: DateTime.UtcNow,
                eventType: NotificationEventType.CommentUpdated.ToString(),
                relatedEntityId: eventArgs.CommentId,
                details: organizerNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(organizerNotificationUpdatedEvent);
        }
    }
}
