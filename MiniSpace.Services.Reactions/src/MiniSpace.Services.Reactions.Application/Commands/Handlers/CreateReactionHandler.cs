using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Services.Clients;
using MiniSpace.Services.Reactions.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class CreateReactionHandler : ICommandHandler<CreateReaction>
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IReactionsOrganizationsEventRepository _orgEventRepository;
        private readonly IReactionsOrganizationsPostRepository _orgPostRepository;
        private readonly IReactionsUserEventRepository _userEventRepository;
        private readonly IReactionsUserPostRepository _userPostRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;


        public CreateReactionHandler(IReactionRepository reactionRepository,
                                     IReactionsOrganizationsEventRepository orgEventRepository,
                                     IReactionsOrganizationsPostRepository orgPostRepository,
                                     IReactionsUserEventRepository userEventRepository,
                                     IReactionsUserPostRepository userPostRepository,
                                     IStudentsServiceClient studentsServiceClient,
                                     IAppContext appContext,
                                     IMessageBroker messageBroker)
        {
            _reactionRepository = reactionRepository;
            _orgEventRepository = orgEventRepository;
            _orgPostRepository = orgPostRepository;
            _userEventRepository = userEventRepository;
            _userPostRepository = userPostRepository;
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

            await EnsureReactionDoesNotExistAsync(command.ContentId, contentType, command.UserId);

            switch (contentType)
            {
                case ReactionContentType.Event when targetType == ReactionTargetType.Organization:
                    if (!await _orgEventRepository.ExistsAsync(command.ContentId))
                        throw new EventNotFoundException(command.ContentId);
                    break;

                case ReactionContentType.Event when targetType == ReactionTargetType.User:
                    if (!await _userEventRepository.ExistsAsync(command.ContentId))
                        throw new EventNotFoundException(command.ContentId);
                    break;

                case ReactionContentType.Post when targetType == ReactionTargetType.Organization:
                    if (!await _orgPostRepository.ExistsAsync(command.ContentId))
                        throw new PostNotFoundException(command.ContentId);
                    break;

                case ReactionContentType.Post when targetType == ReactionTargetType.User:
                    if (!await _userPostRepository.ExistsAsync(command.ContentId))
                        throw new PostNotFoundException(command.ContentId);
                    break;

                default:
                    throw new InvalidReactionContentTypeException(command.ContentType);
            }

            var reaction = Reaction.Create(command.ReactionId, command.UserId, reactionType,
                                           command.ContentId, contentType, targetType);
            await _reactionRepository.AddAsync(reaction);

            await _messageBroker.PublishAsync(new ReactionCreated(command.ReactionId));
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

        private async Task EnsureReactionDoesNotExistAsync(Guid contentId, ReactionContentType contentType, Guid userId)
        {
            if (await _reactionRepository.ExistsAsync(contentId, contentType, userId))
            {
                throw new StudentAlreadyGaveReactionException(userId, contentId, contentType);
            }
        }
    }
}
