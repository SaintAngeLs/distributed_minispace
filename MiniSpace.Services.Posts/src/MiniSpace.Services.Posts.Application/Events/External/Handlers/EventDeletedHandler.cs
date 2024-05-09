using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public EventDeletedHandler(IEventRepository eventRepository, IPostRepository postRepository,
            ICommandDispatcher commandDispatcher)
        {
            _eventRepository = eventRepository;
            _postRepository = postRepository;
            _commandDispatcher = commandDispatcher;
        }
        
        public async Task HandleAsync(EventDeleted @event, CancellationToken cancellationToken = default)
        {
            if (!(await _eventRepository.ExistsAsync(@event.EventId)))
            {
                throw new EventNotFoundException(@event.EventId);
            }
            
            var posts = await _postRepository.GetByEventIdAsync(@event.EventId);
            foreach (var post in posts)
            {
                await _commandDispatcher.SendAsync(new DeletePost(post.Id));
            }

            await _eventRepository.DeleteAsync(@event.EventId);
        }
    }    
}
