using Convey.Logging.CQRS;
using MiniSpace.Services.MediaFiles.Application.Commands;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(UploadMediaFile),  new HandlerLogTemplate
                    {
                        After = "Uploaded media file with ID: {MediaFileId} and name: {FileName}.",
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
