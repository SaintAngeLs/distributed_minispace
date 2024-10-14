using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class DeletePostHandler : ICommandHandler<DeletePost>
    {
        private readonly IPostsService _postsService;
        private readonly IMessageBroker _messageBroker;

        public DeletePostHandler(IPostsService postsService, IMessageBroker messageBroker)
        {
            _postsService = postsService;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeletePost command, CancellationToken cancellationToken = default)
        {
            await _postsService.DeletePostAsync(command);

            await _messageBroker.PublishAsync(new PostDeleted(command.PostId));
        }
    }
}
