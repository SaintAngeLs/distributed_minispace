using Convey.CQRS.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Events.External.Handlers
{
    public class PostCreatedHandler : IEventHandler<PostCreated>
    {
        private readonly IPostRepository _postRepository;

        public PostCreatedHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        
        public async Task HandleAsync(PostCreated @event, CancellationToken cancellationToken)
        {
            if (await _postRepository.ExistsAsync(@event.PostId))
            {
                throw new PostAlreadyAddedException(@event.PostId);
            }
            
            await _postRepository.AddAsync(new Post(@event.PostId));
        }
    }
}