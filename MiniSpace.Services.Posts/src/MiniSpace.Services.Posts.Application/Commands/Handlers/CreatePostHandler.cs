using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class CreatePostHandler : ICommandHandler<CreatePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public CreatePostHandler(
            IPostRepository postRepository,
            IUserPostRepository userPostRepository,
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IDateTimeProvider dateTimeProvider,
            IMessageBroker messageBroker,
            IAppContext appContext)
        {
            _postRepository = postRepository;
            _userPostRepository = userPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreatePost command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.UserId && identity.Id != command.OrganizationId)
            {
                throw new UnauthorizedPostCreationAttemptException(identity.Id, command.EventId ?? Guid.Empty);
            }

            if (command.PostId == Guid.Empty || await _postRepository.ExistsAsync(command.PostId))
            {
                throw new InvalidPostIdException(command.PostId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            if (!Enum.TryParse<VisibilityStatus>(command.Visibility, true, out var visibilityStatus))
            {
                throw new InvalidVisibilityStatusException(command.Visibility);
            }

            var mediaFiles = command.MediaFiles.ToList();
            if (mediaFiles.Count > 12)
            {
                throw new InvalidNumberOfPostMediaFilesException(command.PostId, mediaFiles.Count);
            }

            switch (newState)
            {
                case State.Reported:
                    throw new NotAllowedPostStateException(command.PostId, newState);
                case State.ToBePublished when command.PublishDate is null:
                    throw new PublishDateNullException(command.PostId, newState);
            }

            Post post;

            if (command.Context == PostContext.UserPage)
            {
                post = Post.CreateForUser(command.PostId, command.UserId.Value, command.TextContent, command.MediaFiles,
                    _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                await _userPostRepository.AddAsync(post);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                post = Post.CreateForOrganization(command.PostId, command.OrganizationId.Value, command.TextContent, command.MediaFiles,
                    _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                await _organizationPostRepository.AddAsync(post);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    post = Post.CreateForEvent(command.PostId, command.EventId.Value, command.UserId, command.OrganizationId, command.TextContent,
                        command.MediaFiles, _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                    await _userEventPostRepository.AddAsync(post);
                }
                else
                {
                    post = Post.CreateForEvent(command.PostId, command.EventId.Value, null, command.OrganizationId, command.TextContent,
                        command.MediaFiles, _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                    await _organizationEventPostRepository.AddAsync(post);
                }
            }
            else
            {
                throw new InvalidPostContextException(command.Context.ToString());
            }

            await _messageBroker.PublishAsync(new PostCreated(command.PostId, post.MediaFiles));
        }
    }
}
