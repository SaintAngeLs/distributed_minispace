using Convey.Logging.CQRS;
using MiniSpace.Services.Notifications.Application.Commands;
using MiniSpace.Services.Notifications.Application.Events.External;

namespace MiniSpace.Services.Notifications.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
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
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}
