using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Services.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class CreateReactionHandler : ICommandHandler<CreateReaction>
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
        private readonly ICommentServiceClient _commentServiceClient;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public CreateReactionHandler(
            IReactionRepository reactionRepository,
            IReactionsOrganizationsEventRepository orgEventRepository,
            IReactionsOrganizationsPostRepository orgPostRepository,
            IReactionsUserEventRepository userEventRepository,
            IReactionsUserPostRepository userPostRepository,
            IReactionsOrganizationsEventCommentsRepository orgEventCommentsRepository,
            IReactionsOrganizationsPostCommentsRepository orgPostCommentsRepository,
            IReactionsUserEventCommentsRepository userEventCommentsRepository,
            IReactionsUserPostCommentsRepository userPostCommentsRepository,
            ICommentServiceClient commentServiceClient,
            IStudentsServiceClient studentsServiceClient,
            IAppContext appContext,
            IMessageBroker messageBroker)
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
            _commentServiceClient = commentServiceClient;
            _studentsServiceClient = studentsServiceClient;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateReaction command, CancellationToken cancellationToken = default)
        {
            ValidateStudentIdentity(command);

            await EnsureStudentExistsAsync(command.UserId);

            var contentType = ParseContentType(command.ContentType);
            var targetType = ParseTargetType(command.TargetType);
            var reactionType = ParseReactionType(command.ReactionType);

            var existingReaction = await FindExistingReactionAsync(command.ContentId, command.UserId, contentType, targetType);

            if (existingReaction != null)
            {
                // Update the existing reaction
                existingReaction.UpdateReactionType(reactionType);
                await UpdateReactionAsync(existingReaction, contentType, targetType);
            }
            else
            {
                // Create a new reaction
                var reaction = Reaction.Create(command.ReactionId, command.UserId, reactionType,
                                               command.ContentId, contentType, targetType);

                // Add the reaction to the appropriate repository
                await AddReactionAsync(reaction, contentType, targetType);
            }

            await _messageBroker.PublishAsync(new ReactionCreated(command.ReactionId));
        }

        private async Task<Reaction> FindExistingReactionAsync(Guid contentId, Guid userId, ReactionContentType contentType, ReactionTargetType targetType)
        {
            // Check in each repository if a reaction already exists for the user and content
            switch (contentType)
            {
                case ReactionContentType.Event when targetType == ReactionTargetType.Organization:
                    return await _orgEventRepository.GetAsync(contentId, userId);

                case ReactionContentType.Event when targetType == ReactionTargetType.User:
                    return await _userEventRepository.GetAsync(contentId, userId);

                case ReactionContentType.Post when targetType == ReactionTargetType.Organization:
                    return await _orgPostRepository.GetAsync(contentId, userId);

                case ReactionContentType.Post when targetType == ReactionTargetType.User:
                    return await _userPostRepository.GetAsync(contentId, userId);

                case ReactionContentType.Comment when targetType == ReactionTargetType.Organization:
                    if (await _orgEventCommentsRepository.ExistsAsync(contentId))
                    {
                        return await _orgEventCommentsRepository.GetAsync(contentId, userId);
                    }
                    else if (await _orgPostCommentsRepository.ExistsAsync(contentId))
                    {
                        return await _orgPostCommentsRepository.GetAsync(contentId, userId);
                    }
                    break;

                case ReactionContentType.Comment when targetType == ReactionTargetType.User:
                    if (await _userEventCommentsRepository.ExistsAsync(contentId))
                    {
                        return await _userEventCommentsRepository.GetAsync(contentId, userId);
                    }
                    else if (await _userPostCommentsRepository.ExistsAsync(contentId))
                    {
                        return await _userPostCommentsRepository.GetAsync(contentId, userId);
                    }
                    break;
            }

            return null;
        }

        private async Task AddReactionAsync(Reaction reaction, ReactionContentType contentType, ReactionTargetType targetType)
        {
            switch (contentType)
            {
                case ReactionContentType.Event when targetType == ReactionTargetType.Organization:
                    await _orgEventRepository.AddAsync(reaction);
                    break;

                case ReactionContentType.Event when targetType == ReactionTargetType.User:
                    await _userEventRepository.AddAsync(reaction);
                    break;

                case ReactionContentType.Post when targetType == ReactionTargetType.Organization:
                    await _orgPostRepository.AddAsync(reaction);
                    break;

                case ReactionContentType.Post when targetType == ReactionTargetType.User:
                    await _userPostRepository.AddAsync(reaction);
                    break;

                case ReactionContentType.Comment when targetType == ReactionTargetType.Organization:
                    if (await _orgEventCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _orgEventCommentsRepository.AddAsync(reaction);
                    }
                    else if (await _orgPostCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _orgPostCommentsRepository.AddAsync(reaction);
                    }
                    break;

                case ReactionContentType.Comment when targetType == ReactionTargetType.User:
                    if (await _userEventCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _userEventCommentsRepository.AddAsync(reaction);
                    }
                    else if (await _userPostCommentsRepository.ExistsAsync(reaction.ContentId))
                    {
                        await _userPostCommentsRepository.AddAsync(reaction);
                    }
                    break;

                default:
                    throw new InvalidReactionContentTypeException(reaction.ContentType.ToString());
            }
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
                    throw new InvalidReactionContentTypeException(reaction.ContentType.ToString());
            }
        }

        private void ValidateStudentIdentity(CreateReaction command)
        {
            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != command.UserId)
            {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.UserId);
            }
        }

        private async Task EnsureStudentExistsAsync(Guid studentId)
        {
            if (!await _studentsServiceClient.StudentExistsAsync(studentId))
            {
                throw new StudentNotFoundException(studentId);
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
            if (!Enum.TryParse<ReactionType>(reactionType, false, out var parsedReactionType))
            {
                throw new InvalidReactionTypeException(reactionType);
            }

            return parsedReactionType;
        }
    }
}
