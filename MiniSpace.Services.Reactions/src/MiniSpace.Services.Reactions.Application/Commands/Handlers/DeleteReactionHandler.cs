using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class DeleteReactionHandler(IReactionRepository reactionRepository,
                                 IAppContext appContext,
                                 IMessageBroker messageBroker) : ICommandHandler<DeleteReaction>
    {
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly IMessageBroker _messageBroker = messageBroker;
        private readonly IAppContext _appContext = appContext;
        public async Task HandleAsync(DeleteReaction command, CancellationToken cancellationToken = default)
        {
            _ = await _reactionRepository.GetAsync(command.ReactionId) ??
                throw new ReactionNotFoundException(command.ReactionId);

            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedIdentityException(identity.Id);
            }

            await _reactionRepository.DeleteAsync(command.ReactionId);
            await _messageBroker.PublishAsync(new ReactionDeleted(command.ReactionId));
        }
    }
}
