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
        private readonly IAppContext _appContext;

        public CreateCommentHandler(ICommentRepository commentRepository, IStudentRepository studentRepository,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker, IAppContext appContext)
        {
            _commentRepository = commentRepository;
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateComment command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.StudentId)
            {
                throw new UnauthorizedCommentAccessException(command.ContextId, identity.Id);
            }
            if (!(await _studentRepository.ExistsAsync(command.StudentId)))
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<CommentContext>(command.CommentContext, true, out var newCommentContext))
            {
                throw new InvalidCommentContextEnumException(command.CommentContext);
            }
            
            var parentComment = await _commentRepository.GetAsync(command.ParentId);
            if (parentComment is null)
            {
                throw new ParentCommentNotFoundException(command.ParentId);
            }
            if (parentComment.ParentId != Guid.Empty)
            {
                throw new InvalidParentCommentException(command.ParentId);
            }

            var now = _dateTimeProvider.Now;
            var comment = Comment.Create(command.Id, command.ContextId, newCommentContext, command.StudentId,
                identity.Name, command.ParentId, command.Comment, now);
            await _commentRepository.AddAsync(comment);
            
            parentComment.AddReply(now);
            await _commentRepository.UpdateAsync(parentComment);
            
            await _messageBroker.PublishAsync(new CommentCreated(command.Id));
        }

    }
}
