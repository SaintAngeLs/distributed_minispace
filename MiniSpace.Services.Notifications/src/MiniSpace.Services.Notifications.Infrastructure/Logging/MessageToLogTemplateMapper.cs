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
                 {
                    typeof(FriendRequestCreated), new HandlerLogTemplate
                    {
                        After = "New Friend request created: {NotificationId} "
                    }
                },
            };
        
         public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            if (MessageTemplates.TryGetValue(key, out var template))
            {
                if (template.After.Contains("{NewStatus}"))
                {
                    template.After = template.After.Replace("{NewStatus}", "{Status}");
                }
                return template;
            }
            return null;
        }
    }
}
