using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Events;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class DeleteCommentHandler : ICommandHandler<DeleteComment>
    {
        private readonly IOrganizationEventsCommentRepository _organizationEventsCommentRepository;
        private readonly IOrganizationPostsCommentRepository _organizationPostsCommentRepository;
        private readonly IUserEventsCommentRepository _userEventsCommentRepository;
        private readonly IUserPostsCommentRepository _userPostsCommentRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeleteCommentHandler(
            IOrganizationEventsCommentRepository organizationEventsCommentRepository,
            IOrganizationPostsCommentRepository organizationPostsCommentRepository,
            IUserEventsCommentRepository userEventsCommentRepository,
            IUserPostsCommentRepository userPostsCommentRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationEventsCommentRepository = organizationEventsCommentRepository;
            _organizationPostsCommentRepository = organizationPostsCommentRepository;
            _userEventsCommentRepository = userEventsCommentRepository;
            _userPostsCommentRepository = userPostsCommentRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteComment command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;

            Comment comment = null;

            switch (command.CommentContext)
            {
                case nameof(CommentContext.OrganizationEvent):
                    comment = await _organizationEventsCommentRepository.GetAsync(command.CommentId);
                    break;

                case nameof(CommentContext.OrganizationPost):
                    comment = await _organizationPostsCommentRepository.GetAsync(command.CommentId);
                    break;

                case nameof(CommentContext.UserEvent):
                    comment = await _userEventsCommentRepository.GetAsync(command.CommentId);
                    break;

                case nameof(CommentContext.UserPost):
                    comment = await _userPostsCommentRepository.GetAsync(command.CommentId);
                    break;

                default:
                    throw new InvalidCommentContextEnumException(command.CommentContext);
            }

            if (comment is null)
            {
                throw new CommentNotFoundException(command.CommentId);
            }

            if (identity.IsAuthenticated && identity.Id != comment.UserId)
            {
                throw new UnauthorizedCommentAccessException(command.CommentId, identity.Id);
            }

            comment.Delete();

            switch (command.CommentContext)
            {
                case nameof(CommentContext.OrganizationEvent):
                    await _organizationEventsCommentRepository.UpdateAsync(comment);
                    break;

                case nameof(CommentContext.OrganizationPost):
                    await _organizationPostsCommentRepository.UpdateAsync(comment);
                    break;

                case nameof(CommentContext.UserEvent):
                    await _userEventsCommentRepository.UpdateAsync(comment);
                    break;

                case nameof(CommentContext.UserPost):
                    await _userPostsCommentRepository.UpdateAsync(comment);
                    break;
            }

            // await _messageBroker.PublishAsync(new CommentDeleted(command.CommentId));
        }
    }
}
