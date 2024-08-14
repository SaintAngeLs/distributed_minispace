using System.Diagnostics.CodeAnalysis;
using Convey.Logging.CQRS;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Events.External;

namespace MiniSpace.Services.Posts.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreatePost),  new HandlerLogTemplate
                    {
                        After = "Created the post with id: {PostId}."
                    }
                },
                {
                    typeof(UpdatePost),  new HandlerLogTemplate
                    {
                        After = "Updated the post with id: {PostId}."
                    }
                },
                {
                    typeof(DeletePost), new HandlerLogTemplate
                    {
                        After = "Deleted the post with id: {PostId}."
                    }
                },
                {
                    typeof(ChangePostState), new HandlerLogTemplate
                    {
                        After = "Changed a post with id: {PostId} state to: {State}."
                    }
                },
                {
                    typeof(EventDeleted), new HandlerLogTemplate
                    {
                        After = "Deleted an event with id: {EventId}."
                    }
                },
                {
                    typeof(UpdatePostsState),     
                    new HandlerLogTemplate
                    {
                        After = "Updated state of posts at: {Now}."
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
