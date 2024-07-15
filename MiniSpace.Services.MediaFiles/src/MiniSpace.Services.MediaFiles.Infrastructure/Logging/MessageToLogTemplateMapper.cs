using Convey.Logging.CQRS;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Events.External;

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
                        After = "Uploaded media file with ID: {MediaFileUrl} and name: {FileName}.",
                    }
                },
                {
                    typeof(DeleteMediaFile),  new HandlerLogTemplate
                    {
                        After = "Deleted media file with ID: {MediaFileUrl}.",
                    }
                },
                {
                    typeof(StudentCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated profile picture with ID: {MediaFileId} for student with ID: {StudentId}.",
                    }
                },
                {
                    typeof(StudentUpdated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated profile picture with ID: {MediaFileId} for student with ID: {StudentId}.",
                    }
                },
                {
                    typeof(StudentDeleted),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted all media files for student with ID: {StudentId}.",
                    }
                },
                {
                    typeof(PostCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated media files for post with ID: {PostId}.",
                    }
                },
                {
                    typeof(PostUpdated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated media files for post with ID: {PostId}.",
                    }
                },
                {
                    typeof(PostDeleted),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted all media files for post with ID: {PostId}.",
                    }
                },
                {
                    typeof(EventCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated media files for event with ID: {EventId}.",
                    }
                },
                {
                    typeof(EventUpdated),     
                    new HandlerLogTemplate
                    {
                        After = "Associated media files for event with ID: {EventId}.",
                    }
                },
                {
                    typeof(EventDeleted),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted all media files for event with ID: {EventId}.",
                    }
                },
                {
                    typeof(CleanupUnassociatedFiles),     
                    new HandlerLogTemplate
                    {
                        After = "Cleaned unmatched files for all entities at {Now}."
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
