using Paralax.CQRS.Logging;
using MiniSpace.Services.Email.Application.Commands;
using MiniSpace.Services.Email.Application.Events;
using MiniSpace.Services.Email.Application.Events.External;

namespace MiniSpace.Services.Email.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
       private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateEmailNotification), new HandlerLogTemplate
                    {
                        After = "Sent email with ID: {EmailNotificationId} to user: {UserId}."
                    }
                },
                {
                    typeof(EmailSent), new HandlerLogTemplate
                    {
                        After = "Email successfully sent with ID: {EmailNotificationId} at {SentAt}."
                    }
                },
                {
                    typeof(NotificationCreated), new HandlerLogTemplate
                    {
                        After = "Notification created with ID: {NotificationId} for user: {UserId}. Event type: {EventType} created at {CreatedAt}."
                    }
                }
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
