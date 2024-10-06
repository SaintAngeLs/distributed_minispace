using Paralax.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class DeleteReactionHandler : ICommandHandler<DeleteReaction>
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeleteReactionHandler(IReactionRepository reactionRepository,
                                     IAppContext appContext,
                                     IMessageBroker messageBroker)
        {
            _reactionRepository = reactionRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteReaction command, CancellationToken cancellationToken = default)
        {
            var reaction = await GetReactionAsync(command.ReactionId);

            ValidateUserIdentity(reaction.UserId);

            await _reactionRepository.DeleteAsync(command.ReactionId);

            await _messageBroker.PublishAsync(new ReactionDeleted(command.ReactionId));
        }

        private async Task<Reaction> GetReactionAsync(Guid reactionId)
        {
            var reaction = await _reactionRepository.GetAsync(reactionId);
            if (reaction == null)
            {
                throw new ReactionNotFoundException(reactionId);
            }
            return reaction;
        }

        private void ValidateUserIdentity(Guid studentId)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != studentId)
            {
                throw new UnauthorizedReactionAccessException(identity.Id, studentId);
            }
        }
    }
}
