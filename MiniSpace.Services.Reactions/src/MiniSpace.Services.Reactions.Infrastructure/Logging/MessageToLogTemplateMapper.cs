using System.Diagnostics.CodeAnalysis;
using Convey.Logging.CQRS;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Events;

namespace MiniSpace.Services.Reactions.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateReaction),  new HandlerLogTemplate
                    {
                        After = "Created the reaction with id: {ReactionId}."
                    }
                },
                {
                    typeof(DeleteReaction),  new HandlerLogTemplate
                    {
                        After = "Delete the reaction with id: {ReactionId}."
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
