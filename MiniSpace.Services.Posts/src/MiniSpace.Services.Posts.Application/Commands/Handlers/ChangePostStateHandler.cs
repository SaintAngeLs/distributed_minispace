using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class ChangePostStateHandler : ICommandHandler<ChangePostState>
    {
        private readonly IUserPostRepository _userPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IAppContext _appContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;

        public ChangePostStateHandler(
            IUserPostRepository userPostRepository,
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IAppContext appContext,
            IDateTimeProvider dateTimeProvider,
            IMessageBroker messageBroker)
        {
            _userPostRepository = userPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _appContext = appContext;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(ChangePostState command, CancellationToken cancellationToken = default)
        {
            Post post = null;

            // Determine which repository to use based on the context
            if (command.Context == PostContext.UserPage)
            {
                post = await _userPostRepository.GetAsync(command.PostId);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                post = await _organizationPostRepository.GetAsync(command.PostId);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    post = await _userEventPostRepository.GetAsync(command.PostId);
                }
                else
                {
                    post = await _organizationEventPostRepository.GetAsync(command.PostId);
                }
            }

            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.UserId && identity.Id != post.OrganizationId && !identity.IsAdmin)
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
                case State.Reported:
                    post.SetReported(now);
                    break;
                default:
                    throw new InvalidPostStateException(post.State.ToString().ToLowerInvariant());
            }

            // Update the post in the correct repository
            if (command.Context == PostContext.UserPage)
            {
                await _userPostRepository.UpdateAsync(post);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                await _organizationPostRepository.UpdateAsync(post);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    await _userEventPostRepository.UpdateAsync(post);
                }
                else
                {
                    await _organizationEventPostRepository.UpdateAsync(post);
                }
            }

            await _messageBroker.PublishAsync(new PostStateChanged(post.Id, command.State, previousState));
        }
    }
}
