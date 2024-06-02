using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public EventCreatedHandler(
            IMessageBroker messageBroker,
            IStudentsServiceClient studentsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentsServiceClient = studentsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EventCreated eventCreated, CancellationToken cancellationToken)
        {
            IEnumerable<StudentDto> users;

            try
            {
                users = await _studentsServiceClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                //  Console.WriteLine($"Failed to get users: {ex.Message}");
                throw;
            }

            EventDto eventDetails;
            try
            {
                eventDetails = await _eventsServiceClient.GetEventAsync(eventCreated.EventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve event details: {ex.Message}");
                throw;
            }

            if (users == null)
            {
                //  Console.WriteLine("No users found.");
                return;
            }

            foreach (var user in users)
            {
                var notificationMessage = $"A new event '{eventDetails.Name}' organized by {eventDetails.Organizer.Name} is scheduled from {eventDetails.StartDate:yyyy-MM-dd} to {eventDetails.EndDate:yyyy-MM-dd} with organization {eventDetails.Organizer.OrganizationName} at {eventDetails.Location.Street}, {eventDetails.Location.City} . This event offers a capacity of {eventDetails.Capacity} with a registration fee of ${eventDetails.Fee}. {eventDetails.Description}";
                var detailsHtml = $"<p>Check out the new event details <a href='https://minispace.itsharppro.com/events/{eventCreated.EventId}'>here</a>.</p>";


                var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: user.Id,
                    message: notificationMessage,
                    status: NotificationStatus.Unread,
                    createdAt: DateTime.UtcNow,
                    updatedAt: null,
                    relatedEntityId: eventCreated.EventId,
                    eventType: NotificationEventType.NewEvent
                );

                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(user.Id);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(user.Id);
                }


                studentNotifications.AddNotification(notification);
                await _studentNotificationsRepository.AddOrUpdateAsync(studentNotifications);   

                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: Guid.NewGuid(),
                    userId: user.Id,
                    message: notificationMessage,
                    createdAt:  DateTime.UtcNow,
                    eventType: NotificationEventType.NewEvent.ToString(),
                    relatedEntityId: eventCreated.EventId,
                    details: detailsHtml
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }
        }
    }
}

