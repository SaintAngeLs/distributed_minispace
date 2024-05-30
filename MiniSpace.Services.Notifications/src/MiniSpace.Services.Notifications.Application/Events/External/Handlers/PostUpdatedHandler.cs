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
    public class PostUpdatedHandler : IEventHandler<PostUpdated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IPostsServiceClient _postsServiceClient;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public PostUpdatedHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IEventsServiceClient eventsServiceClient,
            IPostsServiceClient postsServiceClient,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _eventsServiceClient = eventsServiceClient;
            _postsServiceClient = postsServiceClient;
            _studentsServiceClient = studentsServiceClient;
        }

        public async Task HandleAsync(PostUpdated eventArgs, CancellationToken cancellationToken)
        {
            var post = await _postsServiceClient.GetPostAsync(eventArgs.PostId);
            if (post == null)
            {
                Console.WriteLine("Updated post not found.");
                return;
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(post.EventId);
            if (eventDetails == null)
            {
                Console.WriteLine("Event not found for the updated post.");
                return;
            }

            var eventParticipants = await _eventsServiceClient.GetParticipantsAsync(post.EventId);
            if (eventParticipants == null)
            {
                Console.WriteLine("No participants found for the event.");
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

        private async Task NotifyStudent(StudentDto student, EventDto eventDetails, PostDto post)
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
                relatedEntityId: post.EventId,
                eventType: NotificationEventType.PostUpdated,
                details: detailsHtml
            );

            Console.WriteLine($"Creating post update notification for user: {student.Id}");

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
            var notificationMessage = $"An updated post is available for your event '{eventDetails.Name}'.";
            var detailsHtml = $"<p>{notificationMessage} View the updated post here: {post.TextContent}</p>";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: organizer.Id,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: post.EventId,
                eventType: NotificationEventType.PostUpdated,
                details: detailsHtml
            );

            Console.WriteLine($"Creating post update notification for organizer: {organizer.Id}");

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
