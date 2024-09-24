using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class UpdatePostHandler : ICommandHandler<UpdatePost>
    {
        private readonly IPostsService _postsService;
        private readonly IMessageBroker _messageBroker;

        public UpdatePostHandler(IPostsService postsService, IMessageBroker messageBroker)
        {
            _postsService = postsService;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdatePost command, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.UpdatePostAsync(command);

            var postContext = post.UserId.HasValue ? 
                                        "UserPage" : 
                                        post.OrganizationId.HasValue ? "OrganizationPage" : "EventPage";
            var shouldNotify = true; 

            await _messageBroker.PublishAsync(new PostUpdated(
                post.Id,
                post.UserId,
                post.OrganizationId,
                post.EventId,
                post.TextContent,
                post.MediaFiles,
                postContext,
                post.Visibility.ToString(),
                shouldNotify
            ));
        }
    }
}
