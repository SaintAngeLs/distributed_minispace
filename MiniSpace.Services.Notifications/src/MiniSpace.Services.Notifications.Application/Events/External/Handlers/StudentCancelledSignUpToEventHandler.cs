using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class StudentCancelledSignUpToEventHandler : IEventHandler<StudentCancelledSignUpToEvent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;

        public StudentCancelledSignUpToEventHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
        }

        public async Task HandleAsync(StudentCancelledSignUpToEvent eventArgs, CancellationToken cancellationToken)
        {
            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventArgs.StudentId);
            var student = await _studentsServiceClient.GetAsync(eventArgs.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(eventArgs.StudentId);
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.EventId);
            var detailsHtml = eventDetails != null ?
                $"<p>{student.FirstName} {student.LastName}, you have cancelled your registration for the event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>" :
                "<p>Event details could not be retrieved.</p>";

            var notificationMessage = $"You have cancelled your registration for the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.StudentId,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.StudentCancelledSignedUpToEvent,
                details: detailsHtml
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);

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

            if (eventDetails != null && eventDetails.Organizer != null)
            {
                var detailsHtmlForOrganizer = $"<p>{student.FirstName} {student.LastName} has cancelled their registration for your event '{eventDetails.Name}' on {eventDetails.StartDate:yyyy-MM-dd}.</p>";
                var organizerNotification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: eventDetails.Organizer.Id,
                    message: $"{student.FirstName} {student.LastName} has cancelled their registration for your event '{eventDetails.Name}'.",
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventArgs.EventId,
                    eventType: NotificationEventType.StudentCancelledSignedUpToEvent,
                    details: detailsHtmlForOrganizer
                );

                var organizerNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventDetails.Organizer.Id);
                if (organizerNotifications == null)
                {
                    organizerNotifications = new StudentNotifications(eventDetails.Organizer.Id);
                }
               
            

                organizerNotifications.AddNotification(organizerNotification);
                await _studentNotificationsRepository.UpdateAsync(organizerNotifications);

                var organizerNotificationCreatedEvent = new NotificationCreated(
                    notificationId: organizerNotification.NotificationId,
                    userId: organizerNotification.UserId,
                    message: organizerNotification.Message,
                    createdAt: organizerNotification.CreatedAt,
                    eventType: organizerNotification.EventType.ToString(),
                    relatedEntityId: organizerNotification.RelatedEntityId,
                    details: detailsHtmlForOrganizer
                );
                await _messageBroker.PublishAsync(organizerNotificationCreatedEvent);
            }
        }
    }
}