using Convey.CQRS.Commands;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Events.External;

namespace MiniSpace.Services.Notifications.Application.Commands.Handlers
{
    public class CreateNotificationHandler : ICommandHandler<CreateNotification>
    {
        private readonly IStudentNotificationsRepository _studentNotificationsRepository;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IEventsServiceClient _eventsServiceClient;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public CreateNotificationHandler(
            IStudentNotificationsRepository studentNotificationsRepository,
            IFriendsServiceClient friendsServiceClient,
            IEventsServiceClient eventsServiceClient,
            IEventMapper eventMapper, 
            IMessageBroker messageBroker
            )
        {
            _studentNotificationsRepository = studentNotificationsRepository;
            _friendsServiceClient = friendsServiceClient;
             _eventsServiceClient = eventsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateNotification command, CancellationToken cancellationToken = default)
        {
            
            var eventDetails = await _eventsServiceClient.GetEventAsync(command.EventId);
           
            foreach (var userId in command.StudentIds)
            {
                var notification = new Notification(
                    command.NotificationId,
                    userId,
                    command.Message,
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

                 var notificationCreatedEvent = new NotificationCreated(
                    notificationId: notification.NotificationId,
                    userId: userId,
                    message: notification.Message,
                    createdAt: notification.CreatedAt,
                    eventType: notification.EventType.ToString(),
                    relatedEntityId: eventDetails.Id,
                    details: notification.Message 
                );

                await _messageBroker.PublishAsync(notificationCreatedEvent);


                var events = _eventMapper.MapAll(notification.Events);
                await _messageBroker.PublishAsync(events.ToArray());
            }
        }
    }
}
