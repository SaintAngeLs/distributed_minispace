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
    public class UpdatePostHandler : ICommandHandler<UpdatePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdatePostHandler(IPostRepository postRepository, 
            IUserPostRepository userPostRepository, 
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IAppContext appContext, 
            IMessageBroker messageBroker, 
            IDateTimeProvider dateTimeProvider)
        {
            _postRepository = postRepository;
            _userPostRepository = userPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task HandleAsync(UpdatePost command, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(command.PostId);
            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.UserId && identity.Id != post.OrganizationId && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }

            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }

            State? newState = null;
            if (!string.IsNullOrWhiteSpace(command.State))
            {
                if (!Enum.TryParse<State>(command.State, true, out var parsedState))
                {
                    throw new InvalidPostStateException(command.State);
                }
                newState = parsedState;
            }

            VisibilityStatus? newVisibility = null;
            if (!string.IsNullOrWhiteSpace(command.Visibility))
            {
                if (!Enum.TryParse<VisibilityStatus>(command.Visibility, true, out var parsedVisibility))
                {
                    throw new InvalidVisibilityStatusException(command.Visibility);
                }
                newVisibility = parsedVisibility;
            }

            var mediaFiles = command.MediaFiles.ToList();
            if (mediaFiles.Count > 12)
            {
                throw new InvalidNumberOfPostMediaFilesException(post.Id, mediaFiles.Count);
            }

            post.Update(command.TextContent, command.MediaFiles, _dateTimeProvider.Now);

            if (newState.HasValue)
            {
                post.ChangeState(newState.Value, command.PublishDate, _dateTimeProvider.Now);
            }

            if (newVisibility.HasValue)
            {
                post.SetVisibility(newVisibility.Value, _dateTimeProvider.Now);
            }

            await _postRepository.UpdateAsync(post);

            var shouldNotify = true; // Explicitly setting ShouldNotify to true

            var postContext = post.UserId.HasValue ? "UserPage" : post.OrganizationId.HasValue ? "OrganizationPage" : "EventPage";

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
