using Paralax.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class UpdateReactionHandler : ICommandHandler<UpdateReaction>
    {
        private readonly IReactionsOrganizationsEventRepository _orgEventRepository;
        private readonly IReactionsOrganizationsPostRepository _orgPostRepository;
        private readonly IReactionsUserEventRepository _userEventRepository;
        private readonly IReactionsUserPostRepository _userPostRepository;
        private readonly IReactionsOrganizationsEventCommentsRepository _orgEventCommentsRepository;
        private readonly IReactionsOrganizationsPostCommentsRepository _orgPostCommentsRepository;
        private readonly IReactionsUserEventCommentsRepository _userEventCommentsRepository;
        private readonly IReactionsUserPostCommentsRepository _userPostCommentsRepository;
        private readonly IAppContext _appContext;

        public UpdateReactionHandler(
            IReactionsOrganizationsEventRepository orgEventRepository,
            IReactionsOrganizationsPostRepository orgPostRepository,
            IReactionsUserEventRepository userEventRepository,
            IReactionsUserPostRepository userPostRepository,
            IReactionsOrganizationsEventCommentsRepository orgEventCommentsRepository,
            IReactionsOrganizationsPostCommentsRepository orgPostCommentsRepository,
            IReactionsUserEventCommentsRepository userEventCommentsRepository,
            IReactionsUserPostCommentsRepository userPostCommentsRepository,
            IAppContext appContext)
        {
            _orgEventRepository = orgEventRepository;
            _orgPostRepository = orgPostRepository;
            _userEventRepository = userEventRepository;
            _userPostRepository = userPostRepository;
            _orgEventCommentsRepository = orgEventCommentsRepository;
            _orgPostCommentsRepository = orgPostCommentsRepository;
            _userEventCommentsRepository = userEventCommentsRepository;
            _userPostCommentsRepository = userPostCommentsRepository;
            _appContext = appContext;
        }

        public async Task HandleAsync(UpdateReaction command, CancellationToken cancellationToken = default)
        {
            ValidateUserIdentity(command);

            var contentType = ParseContentType(command.ContentType);
            var targetType = ParseTargetType(command.TargetType);

            // Attempt to find the reaction in the appropriate repository using the ReactionId
            var reaction = await FindExistingReactionAsync(command.ReactionId, contentType, targetType);
            if (reaction == null)
            {
                throw new ReactionNotFoundException(command.ReactionId);
            }

            // Ensure the reaction belongs to the correct user
            if (reaction.UserId != command.UserId)
            {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.UserId);
            }

            // Update the reaction type
            var newReactionType = ParseReactionType(command.NewReactionType);
            reaction.UpdateReactionType(newReactionType);

            // Update the reaction in the appropriate repository
            await UpdateReactionAsync(reaction, contentType, targetType);
        }

        private async Task<Reaction> FindExistingReactionAsync(Guid reactionId, ReactionContentType contentType, ReactionTargetType targetType)
        {
            // Retrieve the reaction from the correct repository based on content and target type
            return contentType switch
            {
                ReactionContentType.Event when targetType == ReactionTargetType.Organization => await _orgEventRepository.GetByIdAsync(reactionId),
                ReactionContentType.Event when targetType == ReactionTargetType.User => await _userEventRepository.GetByIdAsync(reactionId),
                ReactionContentType.Post when targetType == ReactionTargetType.Organization => await _orgPostRepository.GetByIdAsync(reactionId),
                ReactionContentType.Post when targetType == ReactionTargetType.User => await _userPostRepository.GetByIdAsync(reactionId),
                ReactionContentType.Comment when targetType == ReactionTargetType.Organization => await _orgEventCommentsRepository.GetByIdAsync(reactionId) ?? await _orgPostCommentsRepository.GetByIdAsync(reactionId),
                ReactionContentType.Comment when targetType == ReactionTargetType.User => await _userEventCommentsRepository.GetByIdAsync(reactionId) ?? await _userPostCommentsRepository.GetByIdAsync(reactionId),
                _ => null,
            };
        }

        private async Task UpdateReactionAsync(Reaction reaction, ReactionContentType contentType, ReactionTargetType targetType)
        {
            switch (contentType)
            {
                case ReactionContentType.Event when targetType == ReactionTargetType.Organization:
                    await _orgEventRepository.UpdateAsync(reaction);
                    break;
                case ReactionContentType.Event when targetType == ReactionTargetType.User:
                    await _userEventRepository.UpdateAsync(reaction);
                    break;
                case ReactionContentType.Post when targetType == ReactionTargetType.Organization:
                    await _orgPostRepository.UpdateAsync(reaction);
                    break;
                case ReactionContentType.Post when targetType == ReactionTargetType.User:
                    await _userPostRepository.UpdateAsync(reaction);
                    break;
                case ReactionContentType.Comment when targetType == ReactionTargetType.Organization:
                    if (await _orgEventCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _orgEventCommentsRepository.UpdateAsync(reaction);
                    }
                    else if (await _orgPostCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _orgPostCommentsRepository.UpdateAsync(reaction);
                    }
                    break;
                case ReactionContentType.Comment when targetType == ReactionTargetType.User:
                    if (await _userEventCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _userEventCommentsRepository.UpdateAsync(reaction);
                    }
                    else if (await _userPostCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _userPostCommentsRepository.UpdateAsync(reaction);
                    }
                    break;
                default:
                    throw new InvalidReactionContentTypeException(contentType.ToString());
            }
        }

        private void ValidateUserIdentity(UpdateReaction command)
        {
            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != command.UserId)
            {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.UserId);
            }
        }

        private ReactionContentType ParseContentType(string contentType)
        {
            if (!Enum.TryParse<ReactionContentType>(contentType, true, out var parsedContentType))
            {
                throw new InvalidReactionContentTypeException(contentType);
            }

            return parsedContentType;
        }

        private ReactionTargetType ParseTargetType(string targetType)
        {
            if (!Enum.TryParse<ReactionTargetType>(targetType, true, out var parsedTargetType))
            {
                throw new InvalidReactionTargetTypeException(targetType);
            }

            return parsedTargetType;
        }

        private ReactionType ParseReactionType(string reactionType)
        {
            if (!Enum.TryParse<ReactionType>(reactionType, true, out var parsedReactionType))
            {
                throw new InvalidReactionTypeException(reactionType);
            }

            return parsedReactionType;
        }
    }
}
