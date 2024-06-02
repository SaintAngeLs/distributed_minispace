using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventsServiceClient _eventsServiceClient;  
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;

        public EventDeletedHandler(
            IMessageBroker messageBroker,
            IEventsServiceClient eventsServiceClient,  
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient)
        {
            _messageBroker = messageBroker;
            _eventsServiceClient = eventsServiceClient;  
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
        }

        
        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            EventDto eventDetails = await _eventsServiceClient.GetEventAsync(eventDeleted.EventId);
            if (eventDetails == null)
            {
                Console.WriteLine("Event details could not be retrieved.");
                return;
            }

            var eventParticipants = await _eventsServiceClient.GetParticipantsAsync(eventDeleted.EventId);
            if (eventParticipants == null)
            {
                Console.WriteLine("No participants found for the event.");
                return;
            }

            
            foreach (var studentParticipant in eventParticipants.SignedUpStudents)
            {
                var student = await _studentsServiceClient.GetAsync(studentParticipant.StudentId);
                if (student == null)
                {
                    continue; // Skip if student details cannot be retrieved
                }

                var notificationMessage = $"The event you were signed up for has been cancelled.";
                var detailsHtml = $"<p>Event <strong>{eventDetails.Name}</strong> scheduled on <strong>{eventDetails.StartDate:yyyy-MM-dd}</strong> has been cancelled. We apologize for any inconvenience.</p>";

                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: student.Id,
                    message: notificationMessage,
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventDeleted.EventId,
                    eventType: NotificationEventType.EventDeleted
                );


                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(student.Id);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(student.Id);
                }
                
                
                studentNotifications.AddNotification(notification);
                await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);

                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: Guid.NewGuid(),
                    userId:  student.Id,
                    message: notificationMessage,
                    createdAt: DateTime.UtcNow,
                    eventType: NotificationEventType.EventDeleted.ToString(),
                    relatedEntityId: eventDeleted.EventId,
                    details: detailsHtml
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }  
        }
    }
}
