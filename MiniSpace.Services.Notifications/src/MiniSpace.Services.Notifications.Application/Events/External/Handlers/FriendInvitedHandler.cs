using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Entities;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using System.Text.Json;

namespace MiniSpace.Services.Notifications.Application.Events.External.Handlers
{
    public class FriendInvitedHandler : 
    // IEventHandler<NotificationCreated>,  
                                        IEventHandler<FriendInvited>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendInvitedHandler(INotificationRepository notificationRepository,  IStudentsServiceClient studentsServiceClient, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _notificationRepository = notificationRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        // public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
        // {
        //     var notification = await _notificationRepository.GetAsync(@event.NotificationId);
        //     if (notification == null)
        //     {
        //         throw new NotificationNotFoundException(@event.NotificationId);
        //     }

        //     await _notificationRepository.AddAsync(notification);
        //     var events = _eventMapper.MapAll(notification.Events);
        //     await _messageBroker.PublishAsync(events.ToArray());
        // }

        public async Task HandleAsync(FriendInvited @event, CancellationToken cancellationToken)
        {
            // Fetch student names based on their IDs
            var inviter = await _studentsServiceClient.GetAsync(@event.InviterId);
            var invitee = await _studentsServiceClient.GetAsync(@event.InviteeId);
               Console.WriteLine("Inviter Object:");
    Console.WriteLine(JsonSerializer.Serialize(inviter, new JsonSerializerOptions { WriteIndented = true }));


            var notificationMessage = $"You have been invited by {inviter.FirstName} {inviter.LastName} to be friends.";

            // Create a notification object based on the event details
            var notification = new Notification(
                notificationId: Guid.NewGuid(),
                userId: @event.InviteeId, // Assuming the invitee should receive the notification
                message: notificationMessage,
                status: NotificationStatus.Unread,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            // Save the notification to the repository
            await _notificationRepository.AddAsync(notification);

            // Create a new event to indicate that a notification has been created
            var notificationCreatedEvent = new NotificationCreated(
                notificationId: notification.NotificationId,
                userId: notification.UserId,
                message: notification.Message,
                createdAt: notification.CreatedAt
            );

            // Publish the NotificationCreated event
            await _messageBroker.PublishAsync(notificationCreatedEvent);
        }
    }
}