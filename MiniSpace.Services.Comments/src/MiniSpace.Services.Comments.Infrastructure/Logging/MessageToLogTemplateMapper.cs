using Paralax.Logging.CQRS;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Events.External;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateComment),  new HandlerLogTemplate
                    {
                        After = "Created the comment with id: {CommentId}."
                    }
                },
                {
                    typeof(UpdateComment),  new HandlerLogTemplate
                    {
                        After = "Updated the comment with id: {CommentId}."
                    }
                },
                {
                    typeof(DeleteComment), new HandlerLogTemplate
                    {
                        After = "Deleted the comment with id: {CommentId}."
                    }
                },
                {
                    typeof(AddLike), new HandlerLogTemplate
                    {
                        After = "Added like in the comment with id: {CommentId}."
                    }
                },
                {
                    typeof(DeleteLike), new HandlerLogTemplate
                    {
                        After = "Removed like in the comment with id: {CommentId}."
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
