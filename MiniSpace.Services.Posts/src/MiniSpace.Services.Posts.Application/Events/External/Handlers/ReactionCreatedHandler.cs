using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using MiniSpace.Services.Posts.Application.Events.External;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class ReactionCreatedHandler : IEventHandler<ReactionCreated>
    {
        private readonly IUserReactionsHistoryRepository _userReactionsHistoryRepository;

        public ReactionCreatedHandler(IUserReactionsHistoryRepository userReactionsHistoryRepository)
        {
            _userReactionsHistoryRepository = userReactionsHistoryRepository;
        }

        public async Task HandleAsync(ReactionCreated @event, CancellationToken cancellationToken = default)
        {
            var eventJson = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                WriteIndented = true 
            });
            Console.WriteLine("Received ReactionCreated event:");
            Console.WriteLine(eventJson);

            var reaction = Reaction.Create(
                @event.ReactionId,
                @event.UserId,
                @event.ReactionType,
                @event.ContentId,
                @event.ContentType,
                @event.TargetType
            );

            await _userReactionsHistoryRepository.SaveReactionAsync(@event.UserId, reaction);
        }
    }
}
