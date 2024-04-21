using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Exceptions;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Commands.Handlers
{
    public class CreateReactionHandler : ICommandHandler<CreateReaction>
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IStudentRepository _studentRepository;
        //private readonly IDateTimeProvider _dateTimeProvider;
        //private readonly IMessageBroker _messageBroker;

        public CreateReactionHandler(IReactionRepository reactionRepository, IStudentRepository studentRepository
            //,IDateTimeProvider dateTimeProvider
            //, IMessageBroker messageBroker
            )
        {
            _reactionRepository = reactionRepository;
            _studentRepository = studentRepository;
            //_dateTimeProvider = dateTimeProvider;
            //_messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateReaction command, CancellationToken cancellationToken = default)
        {
            if (!(await _studentRepository.ExistsAsync(command.StudentId)))
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<ReactionType>(command.ReactionType, true, out var newReactionType))
            {
                throw new InvalidReactionTypeException(command.ReactionType);
            }
            
            var reaction = Reaction.Create(command.StudentId, command.StudentFullName, newReactionType);
            await _reactionRepository.AddAsync(reaction);
            
            //await _messageBroker.PublishAsync(new ReactionCreated(command.ReactionId));
        }
    }
}
