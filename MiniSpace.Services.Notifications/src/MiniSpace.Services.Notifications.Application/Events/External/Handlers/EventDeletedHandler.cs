using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<EventDeletedHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventDeletedHandler(
            IMessageBroker messageBroker,
            IEventsServiceClient eventsServiceClient,
            IStudentNotificationsRepository studentNotificationsRepository,
            IStudentsServiceClient studentsServiceClient,
            ILogger<EventDeletedHandler> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _messageBroker = messageBroker;
            _eventsServiceClient = eventsServiceClient;
            _studentNotificationsRepository = studentNotificationsRepository;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(EventDeleted eventDeleted, CancellationToken cancellationToken)
        {
            EventDto eventDetails;
            try
            {
                eventDetails = await _eventsServiceClient.GetEventAsync(eventDeleted.EventId);
                if (eventDetails == null)
                {
                    _logger.LogError($"Event with ID {eventDeleted.EventId} not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve event details: {ex.Message}");
                return;
            }

            EventParticipantsDto eventParticipants;
            try
            {
                eventParticipants = await _eventsServiceClient.GetParticipantsAsync(eventDeleted.EventId);
                if (eventParticipants == null)
                {
                    _logger.LogError($"No participants found for event with ID {eventDeleted.EventId}.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve participants for event with ID {eventDeleted.EventId}: {ex.Message}");
                return;
            }

            foreach (var studentParticipant in eventParticipants.SignedUpStudents)
            {
                StudentDto student;
                try
                {
                    student = await _studentsServiceClient.GetAsync(studentParticipant.StudentId);
                    if (student == null)
                    {
                        _logger.LogWarning($"Student with ID {studentParticipant.StudentId} not found.");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to retrieve student with ID {studentParticipant.StudentId}: {ex.Message}");
                    continue;
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
                    notification.NotificationId,
                    student.Id,
                    notificationMessage,
                    DateTime.UtcNow,
                    NotificationEventType.EventDeleted.ToString(),
                    eventDeleted.EventId,
                    detailsHtml
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);

                var notificationDto = new NotificationDto
                {
                    UserId = student.Id,
                    Message = notificationMessage,
                    CreatedAt = DateTime.UtcNow,
                    EventType = NotificationEventType.EventDeleted,
                    RelatedEntityId = eventDeleted.EventId,
                    Details = detailsHtml
                };

                await NotificationHub.BroadcastNotification(_hubContext, notificationDto, _logger);
                _logger.LogInformation($"Broadcasted SignalR notification to student with ID {student.Id}.");
            }
        }
    }
}
