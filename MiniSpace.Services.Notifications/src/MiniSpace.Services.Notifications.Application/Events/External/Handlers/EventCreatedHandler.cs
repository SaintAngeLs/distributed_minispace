using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentRepository _userRepository;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public EventCreatedHandler(
            IMessageBroker messageBroker,
            IStudentRepository userRepository,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _userRepository = userRepository;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(EventCreated eventCreated, CancellationToken cancellationToken)
        {

            var users = await _userRepository.GetAllAsync();

            foreach (var user in users)
            {
                 var notification = new Notification(
                    notificationId: Guid.NewGuid(),
                    userId: user.Id,
                    message: $"A new event has been created by Organizer {eventCreated.OrganizerId}",
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
                await _studentNotificationsRepository.UpdateAsync(studentNotifications);

                // Publish the notification event
                var notificationCreatedEvent = new NotificationCreated(
                    notificationId: notification.NotificationId,
                    userId: notification.UserId,
                    message: notification.Message,
                    createdAt: notification.CreatedAt
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);
            }
        }
    }
}
