using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Reactions.Application.Commands;
//using MiniSpace.Services.Reactions.Application.Events.Rejected;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Infrastructure.Exceptions
{
    // internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    // {
    //     public object Map(Exception exception, object message)
    //         => exception switch
    
    //         {
                // InvalidPostContentException ex => message switch
                // {
                //     CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     UpdatePost _ => new UpdatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     _ => null,
                // },
                // InvalidPostPublishDateException ex => message switch
                // {
                //     ChangePostState _ => new ChangePostStateRejected(ex.Id,
                //         ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                //     _ => null,
                // },
                // NotAllowedPostStateException ex => message switch
                // {
                //     CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     _ => null,
                // },
                // PostNotFoundException ex => message switch
                // {
                //     UpdatePost _ => new UpdatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     DeletePost _ => new DeletePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     _ => null,
                // },
                // PostStateAlreadySetException ex => message switch
                // {
                //     ChangePostState _ => new ChangePostStateRejected(ex.Id,
                //         ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                //     _ => null,
                // },
                // PublishDateNullException ex => message switch
                // {
                //     CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     ChangePostState _ => new ChangePostStateRejected(ex.Id,
                //         ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                //     _ => null,
                // },
                // StudentNotFoundException ex => message switch
                // {
                //     CreatePost _ => new CreatePostRejected(ex.Id, ex.Message,
                //         ex.Code),
                //     _ => null,
                // },
                // UnauthorizedPostAccessException ex => message switch
                // {
                //     UpdatePost _ => new UpdatePostRejected(ex.PostId, ex.Message,
                //         ex.Code),
                //     DeletePost _ => new DeletePostRejected(ex.PostId, ex.Message,
                //         ex.Code),
                //     ChangePostState _ => new ChangePostStateRejected(ex.PostId,
                //         "unknown", ex.Message, ex.Code),
                //     _ => null,
                // },
                // UnauthorizedPostOperationException ex => message switch
                // {
                //     UpdatePost _ => new UpdatePostRejected(ex.PostId, ex.Message,
                //         ex.Code),
                //     DeletePost _ => new DeletePostRejected(ex.PostId, ex.Message,
                //         ex.Code),
                //     ChangePostState _ => new ChangePostStateRejected(ex.PostId,
                //         "unknown", ex.Message, ex.Code),
                //     _ => null,
                // },
                // _ => null
            //};
    //}    
}
