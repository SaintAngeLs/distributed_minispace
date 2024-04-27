using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class ChangePostStateHandler : ICommandHandler<ChangePostState>
    {
        private readonly IPostRepository _postRepository;
        private readonly IAppContext _appContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        
        public ChangePostStateHandler(IPostRepository postRepository, IAppContext appContext,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _appContext = appContext;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(ChangePostState command, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(command.PostId);
            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.OrganizerId && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }
            
            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }
            
            if (post.State == newState && post.State != State.ToBePublished)
            {
                throw new PostStateAlreadySetException(post.Id, newState);
            }
            
            var previousState = post.State.ToString().ToLowerInvariant();
            var now = _dateTimeProvider.Now;

            switch (newState)
            {
                case State.ToBePublished:
                    post.SetToBePublished(command.PublishDate
                                          ?? throw new PublishDateNullException(command.PostId, newState), now);
                    break;
                case State.Published:
                    post.SetPublished(now);
                    break;
                case State.InDraft:
                    post.SetInDraft(now);
                    break;
                case State.Hidden:
                    post.SetHidden(now);
                    break;
                case State.Reported:
                    post.SetReported(now);
                    break;
                default:
                    throw new InvalidPostStateException(post.State.ToString().ToLowerInvariant());
            }
            
            await _postRepository.UpdateAsync(post);

            await _messageBroker.PublishAsync(new PostStateChanged(post.Id, command.State, previousState));
        }
    }    
}