using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Events.Application.Events.External;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Events.External.Handlers
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

             var eventJson = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                WriteIndented = true // Optional: For pretty-printing the JSON
            });
            Console.WriteLine("Received CommentCreated event:");
            Console.WriteLine(eventJson);

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

            await _userCommentsHistoryRepository.SaveCommentAsync(@event.UserId, comment);
        }
    }
}
