using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Events.External;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class CommentCreatedHandler : IEventHandler<CommentCreated>
    {
        private readonly IUserCommentsHistoryRepository _userCommentsHistoryRepository;

        public CommentCreatedHandler(IUserCommentsHistoryRepository userCommentsHistoryRepository)
        {
            _userCommentsHistoryRepository = userCommentsHistoryRepository;
        }

        public async Task HandleAsync(CommentCreated @event, CancellationToken cancellationToken = default)
        {
            // Create a new Comment entity based on the received event data
            var comment = new Comment(
                @event.CommentId,
                @event.ContextId,
                @event.CommentContext,
                @event.UserId,
                @event.ParentId,
                @event.TextContent,
                @event.CreatedAt,
                @event.LastUpdatedAt,
                @event.RepliesCount,
                @event.IsDeleted
            );

            // Save the comment to the user's comment history
            await _userCommentsHistoryRepository.SaveCommentAsync(@event.UserId, comment);
        }
    }
}
