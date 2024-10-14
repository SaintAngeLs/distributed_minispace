using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class ViewPostHandler : ICommandHandler<ViewPost>
    {
        private readonly IPostsUserViewsRepository _postsUserViewsRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<ViewPostHandler> _logger;

        public ViewPostHandler(
            IPostsUserViewsRepository postsUserViewsRepository,
            IPostRepository postRepository,
            ILogger<ViewPostHandler> logger)
        {
            _postsUserViewsRepository = postsUserViewsRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ViewPost command, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.ExistsAsync(command.PostId);
            if (!postExists)
            {
                _logger.LogWarning($"Post with ID {command.PostId} not found.");
                return;
            }

            var userViews = await _postsUserViewsRepository.GetAsync(command.UserId);
            if (userViews == null)
            {
                userViews = new PostsViews(command.UserId, Enumerable.Empty<View>());
            }

            var existingView = userViews.Views.FirstOrDefault(v => v.PostId == command.PostId);
            if (existingView != null)
            {
                userViews.RemoveView(command.PostId);
            }

            userViews.AddView(command.PostId, DateTime.UtcNow);

            await _postsUserViewsRepository.UpdateAsync(userViews);

            _logger.LogInformation($"User {command.UserId} viewed post {command.PostId}.");
        }
    }
}
