using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Events.External;
using System;
using System.Text.Json;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class CreateNotificationHandler : ICommandHandler<CreateNotification>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IMessageBroker _messageBroker;

        public CreateNotificationHandler(
            IStudentNotificationsRepository studentNotificationsRepository,
            IFriendsServiceClient friendsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IMessageBroker messageBroker
            )
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _friendsServiceClient = friendsServiceClient;
            _eventsServiceClient = eventsServiceClient;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateNotification command, CancellationToken cancellationToken = default)
        {
            var eventDetails = await _eventsServiceClient.GetEventAsync(command.EventId);
            var eventLink = $"https://minispace.itsharppro.com/events/{eventDetails.Id}";

            foreach (var userId in command.StudentIds)
            {
                var notificationNotDetailed = $"<p>You have been invited to the event '{eventDetails.Name}' " +
                    $"Learn more: <a href='{eventLink}'>Click here</a>.</p>";
                var notificationMessage = $"<p>You have been invited to the event '{eventDetails.Name}' organized by {eventDetails.Organizer.Name}. " +
                    $"This event will take place from {eventDetails.StartDate:yyyy-MM-dd} to {eventDetails.EndDate:yyyy-MM-dd} at {eventDetails.Location.Street}, {eventDetails.Location.City}. " +
                    $"The event has a capacity of {eventDetails.Capacity} and a registration fee of ${eventDetails.Fee}. " +
                    $"Learn more: <a href='{eventLink}'>Click here</a>.</p>";

                var notification = new Notification(
                    command.NotificationId,
                    userId,
                    notificationMessage,
                    NotificationStatus.Unread,
                    DateTime.UtcNow,
                    null,
                    NotificationEventType.NewEventInvitaion,
                    eventDetails.Id,
                    eventDetails?.Name
                );

                var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(userId);
                if (studentNotifications == null)
                {
                    studentNotifications = new StudentNotifications(userId);
                }

                studentNotifications.AddNotification(notification);

                await _studentNotificationsRepository.UpdateAsync(studentNotifications);

                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: notification.NotificationId,
                    userId: userId,
                    message: notificationNotDetailed,
                    createdAt: notification.CreatedAt,
                    eventType: notification.EventType.ToString(),
                    relatedEntityId: eventDetails.Id,
                    details: notificationMessage 
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }
        }
    }
}
