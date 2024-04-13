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
        private readonly IMessageBroker _messageBroker;

        public CreatePostHandler(IPostRepository postRepository, IStudentRepository studentRepository,
            IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _studentRepository = studentRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreatePost command, CancellationToken cancellationToken = default)
        {
            if (!(await _studentRepository.ExistsAsync(command.StudentId)))
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var state))
            {
                throw new InvalidPostStateException(command.State);
            }
            
            var post = Post.Create(command.PostId, command.EventId, command.StudentId,
                command.TextContent, command.MediaContent, state, command.PublishDate);
            await _postRepository.AddAsync(post);
            
            await _messageBroker.PublishAsync(new PostCreated(command.PostId));
        }
    }
}
