using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Events.External.Handlers
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
                throw new StudentAlreadyAddedException(@event.PostId);
            }
            
            await _postRepository.AddAsync(new Post(@event.PostId));
        }
    }
}