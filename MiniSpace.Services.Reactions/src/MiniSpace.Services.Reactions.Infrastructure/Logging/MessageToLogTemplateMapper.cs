using Convey.Logging.CQRS;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Events;

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

                // {
                //     typeof(DeletePost), new HandlerLogTemplate
                //     {
                //         After = "Deleted the post with id: {PostId}."
                //     }
                // },
                // {
                //     typeof(ChangePostState), new HandlerLogTemplate
                //     {
                //         After = "Changed a post with id: {PostId} state to: {State}."
                //     }
                // },
                // {
                //     typeof(StudentCreated), new HandlerLogTemplate
                //     {
                //         After = "Created a new student with id: {StudentId}."
                //     }
                // },
                // {
                //     typeof(StudentDeleted), new HandlerLogTemplate
                //     {
                //         After = "Deleted a student with id: {StudentId}."
                //     }
                // },
                // {
                //     typeof(PostsStateUpdated),     
                //     new HandlerLogTemplate
                //     {
                //         After = "Updated state of posts at: {Now}."
                //     }
                // },
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}
