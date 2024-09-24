using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Entities;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Events;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class RepostPostHandler : ICommandHandler<RepostCommand>
    {
        private readonly IPostsService _postsService;
        private readonly IMessageBroker _messageBroker;

        public RepostPostHandler(IPostsService postsService, IMessageBroker messageBroker)
        {
            _postsService = postsService;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(RepostCommand command, CancellationToken cancellationToken = default)
        {
            var repostedPost = await _postsService.RepostPostAsync(command);

            var shouldNotify = true;

            await _messageBroker.PublishAsync(new PostReposted(
                repostedPost.Id,
                repostedPost.UserId,
                repostedPost.OrganizationId,
                repostedPost.EventId,
                repostedPost.TextContent,
                repostedPost.MediaFiles,
                repostedPost.Context.ToString(),
                repostedPost.Visibility.ToString(),
                shouldNotify
            ));
        }
    }
}
