using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class CreatePostHandler : ICommandHandler<CreatePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public CreatePostHandler(IPostRepository postRepository, IEventRepository eventRepository,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker, IAppContext appContext)
        {
            _postRepository = postRepository;
            _eventRepository = eventRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreatePost command, CancellationToken cancellationToken = default)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && (identity.Id != command.OrganizerId || identity.Id != @event.OrganizerId))
            {
                throw new UnauthorizedPostCreationAttemptException(identity.Id, command.EventId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            switch (newState)
            {
                case State.Reported:
                    throw new NotAllowedPostStateException(command.PostId, newState);
                case State.ToBePublished when command.PublishDate is null:
                    throw new PublishDateNullException(command.PostId, newState);
            }
            
            var post = Post.Create(command.PostId, command.EventId, command.OrganizerId, command.TextContent,
                command.MediaContent, _dateTimeProvider.Now, newState, command.PublishDate);
            await _postRepository.AddAsync(post);
            
            await _messageBroker.PublishAsync(new PostCreated(command.PostId));
        }
    }
}
