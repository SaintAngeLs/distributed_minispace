using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventParticipantRemovedHandler : IEventHandler<EventParticipantRemoved>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;

        public EventParticipantRemovedHandler(
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

        public async Task HandleAsync(EventParticipantRemoved eventArgs, CancellationToken cancellationToken)
        {
            var participantNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventArgs.Participant);
            var participant = await _studentsServiceClient.GetAsync(eventArgs.Participant);
            if (participantNotifications == null)
            {
                participantNotifications = new StudentNotifications(eventArgs.Participant);
            }

            var eventDetails = await _eventsServiceClient.GetEventAsync(eventArgs.EventId);
            var detailsHtml = eventDetails != null ?
                $"<p>You have been removed from the event '{eventDetails.Name}' scheduled on {eventDetails.StartDate:yyyy-MM-dd}.</p>" :
                "<p>Event details could not be retrieved.</p>";

            var notificationMessage = $"You have been removed from the event '{eventDetails.Name}'.";

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.Participant,
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.EventParticipantRemoved,
                details: detailsHtml
            );

            participantNotifications.AddNotification(notification);
            await _studentNotificationsRepository.AddOrUpdateAsync(participantNotifications);

            var notificationCreatedEvent = new NotificationCreated(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.Participant,
                message: notificationMessage,
                createdAt: DateTime.UtcNow,
                eventType: NotificationStatus.Unread.ToString(),
                relatedEntityId: eventArgs.EventId,
                details: detailsHtml
            );

            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}
