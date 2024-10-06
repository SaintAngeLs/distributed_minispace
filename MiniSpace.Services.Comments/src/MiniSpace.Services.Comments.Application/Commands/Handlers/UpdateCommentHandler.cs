using System;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Services.Clients;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class UpdateCommentHandler : ICommandHandler<UpdateComment>
    {
        private readonly IOrganizationEventsCommentRepository _organizationEventsCommentRepository;
        private readonly IOrganizationPostsCommentRepository _organizationPostsCommentRepository;
        private readonly IUserEventsCommentRepository _userEventsCommentRepository;
        private readonly IUserPostsCommentRepository _userPostsCommentRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IStudentsServiceClient _userServiceClient;

        public UpdateCommentHandler(
            IOrganizationEventsCommentRepository organizationEventsCommentRepository,
            IOrganizationPostsCommentRepository organizationPostsCommentRepository,
            IUserEventsCommentRepository userEventsCommentRepository,
            IUserPostsCommentRepository userPostsCommentRepository,
            IAppContext appContext,
            IMessageBroker messageBroker,
            IDateTimeProvider dateTimeProvider,
            IStudentsServiceClient userServiceClient)
        {
            _organizationEventsCommentRepository = organizationEventsCommentRepository;
            _organizationPostsCommentRepository = organizationPostsCommentRepository;
            _userEventsCommentRepository = userEventsCommentRepository;
            _userPostsCommentRepository = userPostsCommentRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
            _userServiceClient = userServiceClient;
        }

        public async Task HandleAsync(UpdateComment command, CancellationToken cancellationToken = default)
        {
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

            if (comment == null)
            {
                throw new CommentNotFoundException(command.CommentId);
            }

            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != comment.UserId)
            {
                throw new UnauthorizedCommentAccessException(command.CommentId, identity.Id);
            }

            comment.Update(command.TextContent, _dateTimeProvider.Now);

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

            var user = await _userServiceClient.GetAsync(identity.Id);
            if (user == null)
            {
                throw new UserNotFoundException(identity.Id);
            }

            await _messageBroker.PublishAsync(new CommentUpdated(
                commentId: command.CommentId,
                userId: identity.Id,
                commentContext: command.CommentContext,
                updatedAt: _dateTimeProvider.Now,
                commentContent: command.TextContent,
                userName: $"{user.FirstName} {user.LastName}",  
                profileImageUrl: user.ProfileImageUrl 
            ));
        }
    }
}
