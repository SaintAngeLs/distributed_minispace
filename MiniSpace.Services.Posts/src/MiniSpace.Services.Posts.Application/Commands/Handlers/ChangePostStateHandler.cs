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
        private readonly IMessageBroker _messageBroker;
        
        public ChangePostStateHandler(IPostRepository postRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(ChangePostState command, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(command.PostId);
            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }
            
            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            if (post.State == newState)
            {
                throw new PostStateAlreadySetException(post.Id, newState);
            }

            var previousState = post.State.ToString().ToLowerInvariant();

            switch (newState)
            {
                case State.ToBePublished:
                    post.SetToBePublished(command.PublishDate ?? throw new ArgumentNullException());
                    break;
                case State.Published:
                    post.SetPublished();
                    break;
                case State.InDraft:
                    post.SetInDraft();
                    break;
                case State.Hidden:
                    post.SetHidden();
                    break;
                case State.Reported:
                    post.SetReported();
                    break;
            }
            
            await _postRepository.UpdateAsync(post);

            await _messageBroker.PublishAsync(new PostStateChanged(post.Id, command.State, previousState));
        }
    }    
}