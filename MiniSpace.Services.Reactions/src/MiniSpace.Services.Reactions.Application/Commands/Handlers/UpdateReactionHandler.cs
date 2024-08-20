using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Services.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class UpdateReactionHandler : ICommandHandler<UpdateReaction>
    {
        private readonly IReactionRepository _reactionRepository;
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
            IReactionRepository reactionRepository,
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
            _reactionRepository = reactionRepository;
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
            ValidateStudentIdentity(command);

            var reaction = await _reactionRepository.GetByIdAsync(command.ReactionId, command.UserId);
            if (reaction == null)
            {
                throw new ReactionNotFoundException(command.ReactionId);
            }

            if (reaction.UserId != command.UserId)
            {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.UserId);
            }

            var newReactionType = ParseReactionType(command.NewReactionType);
            reaction.UpdateReactionType(newReactionType);

            var contentType = reaction.ContentType;
            var targetType = reaction.TargetType;

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

        private void ValidateStudentIdentity(UpdateReaction command)
        {
            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != command.UserId)
            {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.UserId);
            }
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
