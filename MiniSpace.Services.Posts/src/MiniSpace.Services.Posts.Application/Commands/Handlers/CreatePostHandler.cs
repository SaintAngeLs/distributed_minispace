using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class CreatePostHandler : ICommandHandler<CreatePost>
    {
        private readonly IPostsService _postsService;
        private readonly IMessageBroker _messageBroker;

        public CreatePostHandler(IPostsService postsService, IMessageBroker messageBroker)
        {
            _postsService = postsService;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreatePost command, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.CreatePostAsync(command);

            await _messageBroker.PublishAsync(new PostCreated(
                command.PostId,
                command.UserId,
                command.OrganizationId,
                command.EventId,
                command.TextContent,
                command.MediaFiles,
                command.Context.ToString(),
                command.Visibility,
                shouldNotify: true
            ));
        }
    }
}
