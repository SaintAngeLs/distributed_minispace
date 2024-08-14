using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class DeletePostHandler : ICommandHandler<DeletePost>
    {
        private readonly IUserPostRepository _userPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeletePostHandler(
            IUserPostRepository userPostRepository,
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _userPostRepository = userPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeletePost command, CancellationToken cancellationToken = default)
        {
            Post post = null;

            switch (command.Context.ToLowerInvariant())
            {
                case "userpage":
                    post = await _userPostRepository.GetAsync(command.PostId);
                    break;
                case "organizationpage":
                    post = await _organizationPostRepository.GetAsync(command.PostId);
                    break;
                case "eventpage" when command.UserId.HasValue:
                    post = (await _userEventPostRepository.GetByUserEventIdAsync(command.UserId.Value, command.EventId.Value))
                        .FirstOrDefault(p => p.Id == command.PostId);
                    break;
                case "eventpage" when command.OrganizationId.HasValue:
                    post = (await _organizationEventPostRepository.GetByOrganizationEventIdAsync(command.OrganizationId.Value, command.EventId.Value))
                        .FirstOrDefault(p => p.Id == command.PostId);
                    break;
                default:
                    throw new InvalidPostContextException(command.Context);
            }

            if (post == null)
            {
                throw new PostNotFoundException(command.PostId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != (post.UserId ?? post.OrganizationId) && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }

            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }

            switch (command.Context.ToLowerInvariant())
            {
                case "userpage":
                    await _userPostRepository.DeleteAsync(command.PostId);
                    break;
                case "organizationpage":
                    await _organizationPostRepository.DeleteAsync(command.PostId);
                    break;
                case "eventpage" when command.UserId.HasValue:
                    await _userEventPostRepository.DeleteAsync(command.PostId);
                    break;
                case "eventpage" when command.OrganizationId.HasValue:
                    await _organizationEventPostRepository.DeleteAsync(command.PostId);
                    break;
                default:
                    throw new InvalidPostContextException(command.Context);
            }

            // Publish the post deleted event
            await _messageBroker.PublishAsync(new PostDeleted(command.PostId));
        }
    }
}
