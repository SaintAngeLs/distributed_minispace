using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class CreateReactionHandler(IReactionRepository reactionRepository,
                                 IPostRepository postRepository,
                                 IEventRepository eventRepository,
                                 IStudentRepository studentRepository,
                                 IAppContext appContext,
                                 IMessageBroker messageBroker
                                 ) : ICommandHandler<CreateReaction>
    {
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IStudentRepository _studentRepository = studentRepository;
        private readonly IEventRepository _eventRepository = eventRepository;
        private readonly IMessageBroker _messageBroker = messageBroker;
        private readonly IAppContext _appContext = appContext;

        public async Task HandleAsync(CreateReaction command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != command.StudentId) {
                throw new UnauthorizedReactionAccessException(command.ReactionId, command.StudentId);
            }

            if (!await _studentRepository.ExistsAsync(command.StudentId))
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            // Check the content type
            if (!Enum.TryParse<ReactionContentType>(command.ContentType, true, out var contentType)) {
                throw new InvalidReactionContentTypeException(command.ContentType);
            }

            // Check the content
            switch (contentType) {
                case ReactionContentType.Event:
                    if (!await _eventRepository.ExistsAsync(command.ContentId)) {
                        throw new EventNotFoundException(command.ContentId);
                    }
                    break;
                case ReactionContentType.Post:
                    if (!await _postRepository.ExistsAsync(command.ContentId)) {
                        throw new PostNotFoundException(command.ContentId);
                    }
                    break;
                default:
                    break;
            }

            // check the reaction type
            // case-sensitive
            if (!Enum.TryParse<ReactionType>(command.ReactionType, false, out var reactionType))
            {
                throw new InvalidReactionTypeException(command.ReactionType);
            }

            if (await _reactionRepository.ExistsAsync(command.ContentId, contentType, command.StudentId))
            {
                throw new StudentAlreadyGaveReactionException(command.StudentId, command.ContentId, contentType);
            }

            string studentFullName = identity.Name;
            
            var reaction = Reaction.Create(command.ReactionId, command.StudentId, studentFullName, reactionType, 
                command.ContentId, contentType);
            await _reactionRepository.AddAsync(reaction);
            
            await _messageBroker.PublishAsync(new ReactionCreated(command.ReactionId));
        }
    }
}
