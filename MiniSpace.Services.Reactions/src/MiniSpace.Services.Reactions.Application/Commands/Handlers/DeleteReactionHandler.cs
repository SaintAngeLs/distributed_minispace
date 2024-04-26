using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    // public class DeleteReactionHandler : ICommandHandler<DeleteReaction>
    // {
    //     private readonly IStudentRepository _studentRepository;
    //     //private readonly IEventRepository _eventRepository;
    //     private readonly IPostRepository _postRepository;

    //     public DeleteReactionHandler(IStudentRepository studentRepository,
    //                              //IEventRepository eventRepository,
    //                              IPostRepository postRepository)
    //     {
    //         _studentRepository = studentRepository;
    //         //_eventRepository = eventRepository;
    //         _postRepository = postRepository;
    //     }
        
    //     public async Task HandleAsync(DeleteReaction command, CancellationToken cancellationToken = default)
    //     {
    //         switch (command.ContentType) {
    //             case ReactionContentType.Event:
    //                 var @event = await _eventRepository.GetAsync(command.ContentId);
    //                 if (@event is null) {
    //                     throw new EventNotFoundException(command.ContentId);
    //                 }
    //                 break;
    //             // case ReactionContentType.Post:
    //             //     var post = await _postRepository.GetAsync(command.ContentId);
    //             //     if (post is null) {
    //             //         throw new PostNotFoundException(command.ContentId);
    //             //     }
    //             //     break;
    //         }

    //         //await _postRepository.DeleteAsync(command.PostId);
    //     }
    // }    
}
