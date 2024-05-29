using System;
using System.Threading.Tasks;
using System.Threading;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.DTO;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class PostCreatedHandler : IEventHandler<PostCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;

        public PostCreatedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
        }

        public async Task HandleAsync(PostCreated eventArgs, CancellationToken cancellationToken)
        {
            var post = await _postsServiceClient.GetPostAsync(eventArgs.PostId);
            if (post == null)
            {
                Console.WriteLine("Post not found.");
                return;
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(post.EventId);
            if (eventDetails == null)
            {
                Console.WriteLine("Event not found for the post.");
                return;
            }

            // Notify interested students
            IEnumerable<StudentDto> interestedStudents;
            try
            {
                interestedStudents = await _eventsServiceClient.GetParticipantsAsync(post.EventId);  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get interested students: {ex.Message}");
                throw;
            }

            if (interestedStudents == null)
            {
                Console.WriteLine("No interested students found for the event.");
                return;
            }

            foreach (var student in interestedStudents)
            {
                await NotifyStudent(student, eventDetails, post);
            }

            // Notify the organizer
            await NotifyOrganizer(eventDetails.Organizer, eventDetails, post);
        }

        private async Task NotifyStudent(StudentDto student, EventDto eventDetails, PostDto post)
        {
            var notificationMessage = $"A new post has been created for an event you are interested in: '{eventDetails.Name}'.";
            var detailsHtml = $"<p>{notificationMessage} Check out the post details here: {post.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: student.Id,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: post.EventId,
                eventType: NotificationEventType.PostCreated,
                details: detailsHtml
            );

            Console.WriteLine($"Creating post creation notification for user: {student.Id}");

            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(student.Id);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(student.Id);
            }
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt,
                eventType: notification.EventType.ToString(),
                relatedEntityId: notification.RelatedEntityId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);


            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

            
        }

        private async Task NotifyOrganizer(OrganizerDto organizer, EventDto eventDetails, PostDto post)
        {
            var notificationMessage = $"A new post has been created for your event '{eventDetails.Name}'.";
            var detailsHtml = $"<p>{notificationMessage} View the new post here: {post.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: organizer.Id,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: post.EventId,
                eventType: NotificationEventType.PostCreated,
                details: detailsHtml
            );

            Console.WriteLine($"Creating post creation notification for organizer: {organizer.Id}");

            var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(organizer.Id);
            if (organizerNotifications == null)
            {
                organizerNotifications = new StudentNotifications(organizer.Id);
            }
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt,
                eventType: notification.EventType.ToString(),
                relatedEntityId: notification.RelatedEntityId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
            

            organizerNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(organizerNotifications);

            
        }
    }
}
