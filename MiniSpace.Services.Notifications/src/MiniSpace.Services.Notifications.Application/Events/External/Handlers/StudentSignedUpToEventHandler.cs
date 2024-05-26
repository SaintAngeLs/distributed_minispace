using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class StudentSignedUpToEventHandler : IEventHandler<StudentSignedUpToEvent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;

        public StudentSignedUpToEventHandler(
            IMessageBroker messageBroker,
            IStudentNotificationsRepository studentNotificationsRepository)
        {
            _messageBroker = messageBroker;
            _studentNotificationsRepository = studentNotificationsRepository;
        }

        public async Task HandleAsync(StudentSignedUpToEvent eventArgs, CancellationToken cancellationToken)
        {
            var studentNotifications = await _studentNotificationsRepository.GetByStudentIdAsync(eventArgs.StudentId);
            if (studentNotifications == null)
            {
                studentNotifications = new StudentNotifications(eventArgs.StudentId);
            }

            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: eventArgs.StudentId,
                message: $"You have successfully signed up for the event {eventArgs.EventId}.",
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                relatedEntityId: eventArgs.EventId,
                eventType: NotificationEventType.EventNewSignUp
            );

            studentNotifications.AddNotification(notification);
            await _studentNotificationsRepository.UpdateAsync(studentNotifications);
            
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
