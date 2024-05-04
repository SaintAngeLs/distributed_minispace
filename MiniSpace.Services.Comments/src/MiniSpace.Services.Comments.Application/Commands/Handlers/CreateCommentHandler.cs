using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Exceptions;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class CreateCommentHandler : ICommandHandler<CreateComment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;

        public CreateCommentHandler(ICommentRepository commentRepository, IStudentRepository studentRepository,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker)
        {
            _commentRepository = commentRepository;
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateComment command, CancellationToken cancellationToken = default)
        {
            if (!(await _studentRepository.ExistsAsync(command.StudentId)))
            {
                throw new StudentNotFoundException(command.StudentId);
            }
            
            var comment = Comment.Create(command.Id, command.ContextId, command.StudentId,
                command.Likes, command.ParentId, command.Comment, _dateTimeProvider.Now);
            await _commentRepository.AddAsync(comment);
            
            await _messageBroker.PublishAsync(new CommentCreated(command.Id));
        }

    }
}
