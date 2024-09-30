using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class UpdatePostsStateHandler : ICommandHandler<UpdatePostsState>
    {
        private IPostRepository _postRepository;
        private IMessageBroker _messageBroker;

        public UpdatePostsStateHandler(IPostRepository postRepository, IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdatePostsState command, CancellationToken cancellationToken)
        {
            var posts = (await _postRepository.GetToUpdateAsync()).ToList();
            foreach (var @post in posts)
            {
                if (@post.UpdateState(command.Now))
                    await _postRepository.UpdateAsync(@post);
            }

            await _messageBroker.PublishAsync(new PostsStateUpdated(command.Now));
        }
    }
}