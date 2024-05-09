using Convey.Logging.CQRS;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Events.External;

namespace MiniSpace.Services.Reactions.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateReaction),  new HandlerLogTemplate
                    {
                        After = "Created the reaction with student id: {StudentId} and content id: {ContentId}."
                    }
                },
                {
                    typeof(DeleteReaction),  new HandlerLogTemplate
                    {
                        After = "Delete the reaction with student id: {StudentId} and content id: {ContentId}."
                    }
                },
                {
                    typeof(ReactionCreated), new HandlerLogTemplate
                    {
                        After = "Created a new reaction with id: {ReactionId}."
                    }
                },
                {
                    typeof(ReactionDeleted), new HandlerLogTemplate
                    {
                        After = "Deleted a new reaction with id: {ReactionId}."
                    }
                },
                {
                    typeof(EventCreated), new HandlerLogTemplate
                    {
                        After = "Created a new event with id: {EventId}."
                    }
                },
                {
                    typeof(PostCreated), new HandlerLogTemplate
                    {
                        After = "Created a new post with id: {PostId}."
                    }
                },
                {
                    typeof(StudentCreated), new HandlerLogTemplate
                    {
                        After = "Created a new student with id: {StudentId}."
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
