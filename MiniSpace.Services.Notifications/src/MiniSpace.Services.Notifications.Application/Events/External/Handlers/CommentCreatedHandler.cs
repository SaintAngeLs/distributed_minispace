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
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly ICommentsServiceClient _commentsServiceClient;

        public CommentCreatedHandler(
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

        public async Task HandleAsync(CommentCreated eventArgs, CancellationToken cancellationToken)
        {
            CommentDto commentDetails;
            try
            {
                commentDetails = await _commentsServiceClient.GetCommentAsync(eventArgs.CommentId);
                if (commentDetails == null)
                {
                    Console.WriteLine("No comment details found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve comment details: {ex.Message}");
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

            // Notification creation logic for the user who posted the comment
            var userNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: commentDetails.StudentId,
                message: $"Thank you for your comment on the event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentCreated
            );

            studentNotifications.AddNotification(userNotification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);   
            
            // Prepare detailed HTML message
            var userNotificationDetailsHtml = $"<p>Your comment on the event '{eventDetails.Name}' has been posted successfully.</p>";

            // Publishing the event
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: userNotification.NotificationId,
                userId: userNotification.UserId,
                message: userNotification.Message,
                createdAt: userNotification.CreatedAt,
                eventType: "CommentCreated",
                relatedEntityId: userNotification.RelatedEntityId,
                details: userNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);

            var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventDetails.Organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new StudentNotifications(eventDetails.Organizer.Id);
            }


            // Repeat similar logic for the organizer or other relevant parties
            var organizerNotification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"A new comment has been posted by {commentDetails.StudentName} on your event '{eventDetails.Name}'.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.CommentId,
                eventType: NotificationEventType.CommentCreated
            );

            organizerNotifications.AddNotification(organizerNotification);
            await _studentNotificationsRepository.UpdateAsync(organizerNotifications); 
            
            var organizerNotificationDetailsHtml = $"<p>{commentDetails.StudentName} commented on your event '{eventDetails.Name}': {commentDetails.CommentContext}</p>";

            var organizerNotificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: eventDetails.Organizer.Id,
                message: $"A new comment has been posted by {commentDetails.StudentName} on your event '{eventDetails.Name}'.",
                createdAt: organizerNotification.CreatedAt,
                eventType: NotificationEventType.CommentCreated.ToString(),
                relatedEntityId: eventArgs.CommentId,
                details: organizerNotificationDetailsHtml
            );

            await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);
        }
    }
}
