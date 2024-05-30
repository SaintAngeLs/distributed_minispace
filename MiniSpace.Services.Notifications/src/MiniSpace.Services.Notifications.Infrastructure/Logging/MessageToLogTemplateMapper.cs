using Convey.Logging.CQRS;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Application.Commands;
using MiniSpace.Services.Notifications.Application.Events.External;

namespace MiniSpace.Services.Notifications.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
       private readonly ILogger<MessageToLogTemplateMapper> _logger;
       private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateNotification), new HandlerLogTemplate
                    {
                        After = "Created notification with id: {NotificationId} for user: {UserId}."
                    }
                },
                {
                    typeof(DeleteNotification), new HandlerLogTemplate
                    {
                        After = "Deleted notification with id: {NotificationId}."
                    }
                },
                {
                    typeof(UpdateNotificationStatus), new HandlerLogTemplate
                    {
                        After = "Updated the status of notification with id: {NotificationId} to: {NewStatus}."
                    }
                },
                {
                    typeof(FriendRequestCreated), new HandlerLogTemplate
                    {
                        After = "Processed creation of friend request from {RequesterId} to {FriendId}."
                    }
                },
                {
                    typeof(FriendRequestSent), new HandlerLogTemplate
                    {
                        After = "Processed friend request sent from {InviterId} to {InviteeId}."
                    }
                },
                {
                    typeof(FriendInvited), new HandlerLogTemplate
                    {
                        After = "Handled invitation sent by {InviterId} to {InviteeId}."
                    }
                },
                {
                    typeof(NotificationCreated), new HandlerLogTemplate
                    {
                        After = "Notification created with ID: {NotificationId} for user: {UserId}, message: '{Message}' at {CreatedAt}."
                    }
                },
            };
        
        public MessageToLogTemplateMapper(ILogger<MessageToLogTemplateMapper> logger)
        {
            _logger = logger;
        }

        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var messageType = message.GetType();
            _logger.LogInformation($"Attempting to map message type: {messageType.Name}");
            if (MessageTemplates.TryGetValue(messageType, out var template))
            {
                _logger.LogInformation($"Mapping found. Template: {template.After}");
                return template;
            }
            _logger.LogWarning($"No mapping found for message type: {messageType.Name}");
            return null;
        }
    }
}
