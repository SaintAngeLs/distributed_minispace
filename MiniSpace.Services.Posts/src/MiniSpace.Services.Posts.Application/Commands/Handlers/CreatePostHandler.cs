using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class CreatePostHandler : ICommandHandler<CreatePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;

        public CreatePostHandler(IPostRepository postRepository, IStudentRepository studentRepository,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreatePost command, CancellationToken cancellationToken = default)
        {
            if (!(await _studentRepository.ExistsAsync(command.StudentId)))
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            switch (newState)
            {
                case State.Hidden or State.Reported:
                    throw new NotAllowedPostStateException(command.PostId, newState);
                case State.ToBePublished when command.PublishDate is null:
                    throw new PublishDateNullException(command.PostId, newState);
            }
            
            var post = Post.Create(command.PostId, command.EventId, command.StudentId, command.TextContent,
                command.MediaContent, _dateTimeProvider.Now, newState, command.PublishDate);
            await _postRepository.AddAsync(post);
            
            await _messageBroker.PublishAsync(new PostCreated(command.PostId));
        }
    }
}
