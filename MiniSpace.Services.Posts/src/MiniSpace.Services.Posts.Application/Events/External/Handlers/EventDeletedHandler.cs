using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public EventDeletedHandler(
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            ICommandDispatcher commandDispatcher)
        {
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(EventDeleted @event, CancellationToken cancellationToken = default)
        {
            var userPosts = await _userEventPostRepository.GetByEventIdAsync(@event.EventId);
            foreach (var post in userPosts)
            {
                await _commandDispatcher.SendAsync(new DeletePost(post.Id, post.UserId, null, post.EventId, PostContext.EventPage.ToString()));
            }

            var organizationPosts = await _organizationEventPostRepository.GetByEventIdAsync(@event.EventId);
            foreach (var post in organizationPosts)
            {
                await _commandDispatcher.SendAsync(new DeletePost(post.Id, null, post.OrganizationId, post.EventId, PostContext.EventPage.ToString()));
            }
        }
    }
}
