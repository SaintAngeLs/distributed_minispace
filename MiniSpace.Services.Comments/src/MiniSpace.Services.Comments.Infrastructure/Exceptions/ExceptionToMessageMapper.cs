using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Events.Rejected;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Core.Exceptions;

namespace MiniSpace.Services.Comments.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
    
            {
                /*InvalidPostContentException ex => message switch
                {
                    CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    UpdatePost _ => new UpdatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                InvalidPostPublishDateException ex => message switch
                {
                    ChangePostState _ => new ChangePostStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    _ => null,
                },
                NotAllowedPostStateException ex => message switch
                {
                    CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                PostNotFoundException ex => message switch
                {
                    UpdatePost _ => new UpdatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    DeletePost _ => new DeletePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                PostStateAlreadySetException ex => message switch
                {
                    ChangePostState _ => new ChangePostStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    _ => null,
                },
                PublishDateNullException ex => message switch
                {
                    CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    ChangePostState _ => new ChangePostStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    _ => null,
                },
                StudentNotFoundException ex => message switch
                {
                    CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                UnauthorizedPostAccessException ex => message switch
                {
                    UpdatePost _ => new UpdatePostRejected(ex.PostId, ex.Message,
                        ex.Code),
                    DeletePost _ => new DeletePostRejected(ex.PostId, ex.Message,
                        ex.Code),
                    ChangePostState _ => new ChangePostStateRejected(ex.PostId,
                        "unknown", ex.Message, ex.Code),
                    _ => null,
                },
                UnauthorizedPostOperationException ex => message switch
                {
                    UpdatePost _ => new UpdatePostRejected(ex.PostId, ex.Message,
                        ex.Code),
                    DeletePost _ => new DeletePostRejected(ex.PostId, ex.Message,
                        ex.Code),
                    ChangePostState _ => new ChangePostStateRejected(ex.PostId,
                        "unknown", ex.Message, ex.Code),
                    _ => null,
                },*/
                _ => null
            };
    }    
}
