using Paralax.CQRS.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Events.External.Handlers
{
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly ICommentRepository _commentRepository;

        public CommentCreatedHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        
        public async Task HandleAsync(CommentCreated @event, CancellationToken cancellationToken)
        {
            if (await _commentRepository.ExistsAsync(@event.CommentId))
            {
                throw new CommentAlreadyAddedException(@event.CommentId);
            }
            
            await _commentRepository.AddAsync(new Comment(@event.CommentId));
        }
    }
}