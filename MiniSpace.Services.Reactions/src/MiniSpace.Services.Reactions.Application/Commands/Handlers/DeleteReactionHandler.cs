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
                                 IStudentRepository studentRepository,
                                 IPostRepository postRepository,
                                 IEventRepository eventRepository,
                                 IDateTimeProvider dateTimeProvider,
                                 //IAppContext appContext,
                                 IMessageBroker messageBroker) : ICommandHandler<DeleteReaction>
    {
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly IStudentRepository _studentRepository = studentRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IEventRepository _eventRepository = eventRepository;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IMessageBroker _messageBroker = messageBroker;
        //private readonly IAppContext _appContext = appContext;
        public async Task HandleAsync(DeleteReaction command, CancellationToken cancellationToken = default)
        {
            var reaction = await _reactionRepository.GetAsync(command.StudentId, command.ContentId, command.ContentType);
            if (reaction is null) {
                throw new ReactionNotFoundException(command.StudentId, command.ContentId, command.ContentType);
            }

            await _reactionRepository.DeleteAsync(command.StudentId, command.ContentId, command.ContentType);
            await _messageBroker.PublishAsync(new ReactionDeleted(command.StudentId, command.ContentId, command.ContentType));
        }
    }
}
