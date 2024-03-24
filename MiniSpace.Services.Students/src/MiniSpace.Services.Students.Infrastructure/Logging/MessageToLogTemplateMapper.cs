using Convey.Logging.CQRS;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.External;

namespace MiniSpace.Services.Students.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(UpdateStudent),  new HandlerLogTemplate
                    {
                        After = "Updated the student with id: {Id}."
                    }
                },
                {
                    typeof(DeleteStudent), new HandlerLogTemplate
                    {
                        After = "Deleted the student with id: {Id}."
                    }
                },
                {
                    typeof(SignedUp), new HandlerLogTemplate
                    {
                        After = "Created a new student with id: {UserId}."
                    }
                }
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}
