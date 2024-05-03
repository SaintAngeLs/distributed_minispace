using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class UpdateLikeHandler : ICommandHandler<UpdateLike>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public UpdateLikeHandler(ICommentRepository commentRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _commentRepository = commentRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateLike command, CancellationToken cancellationToken = default)
        {
            var comment = await _commentRepository.GetAsync(command.CommentId);
            if (comment is null)
            {
                throw new CommentNotFoundException(command.CommentId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                throw new UnauthorizedCommentAccessException(command.CommentId, identity.Id);
            }

            comment.Like(identity.Id);
            await _commentRepository.UpdateAsync(comment);

            await _messageBroker.PublishAsync(new CommentUpdated(command.CommentId));
        }
    }
}
