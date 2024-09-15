using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Application.Services.Clients;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Exceptions;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class CreateCommentHandler : ICommandHandler<CreateComment>
    {
        private readonly IOrganizationEventsCommentRepository _organizationEventsCommentRepository;
        private readonly IOrganizationPostsCommentRepository _organizationPostsCommentRepository;
        private readonly IUserEventsCommentRepository _userEventsCommentRepository;
        private readonly IUserPostsCommentRepository _userPostsCommentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;
        private readonly IStudentsServiceClient _userServiceClient;

        public CreateCommentHandler(
            IOrganizationEventsCommentRepository organizationEventsCommentRepository,
            IOrganizationPostsCommentRepository organizationPostsCommentRepository,
            IUserEventsCommentRepository userEventsCommentRepository,
            IUserPostsCommentRepository userPostsCommentRepository,
            IStudentsServiceClient userServiceClient,
            IDateTimeProvider dateTimeProvider,
            IMessageBroker messageBroker,
            IAppContext appContext)
        {
            _organizationEventsCommentRepository = organizationEventsCommentRepository;
            _organizationPostsCommentRepository = organizationPostsCommentRepository;
            _userEventsCommentRepository = userEventsCommentRepository;
            _userPostsCommentRepository = userPostsCommentRepository;
            _userServiceClient = userServiceClient;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateComment command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;

            if (identity.IsAuthenticated && identity.Id != command.UserId)
            {
                throw new UnauthorizedCommentAccessException(command.ContextId, identity.Id);
            }

            var user = await _userServiceClient.GetAsync(command.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            if (!Enum.TryParse<CommentContext>(command.CommentContext, true, out var commentContext))
            {
                throw new InvalidCommentContextEnumException(command.CommentContext);
            }

            var now = _dateTimeProvider.Now;
            Comment comment;

            switch (commentContext)
            {
                case CommentContext.OrganizationEvent:
                    comment = Comment.Create(command.CommentId, command.ContextId, commentContext, command.UserId, command.ParentId, command.TextContent, now);
                    await _organizationEventsCommentRepository.AddAsync(comment);
                    break;

                case CommentContext.OrganizationPost:
                    comment = Comment.Create(command.CommentId, command.ContextId, commentContext, command.UserId, command.ParentId, command.TextContent, now);
                    await _organizationPostsCommentRepository.AddAsync(comment);
                    break;

                case CommentContext.UserEvent:
                    comment = Comment.Create(command.CommentId, command.ContextId, commentContext, command.UserId, command.ParentId, command.TextContent, now);
                    await _userEventsCommentRepository.AddAsync(comment);
                    break;

                case CommentContext.UserPost:
                    comment = Comment.Create(command.CommentId, command.ContextId, commentContext, command.UserId, command.ParentId, command.TextContent, now);
                    await _userPostsCommentRepository.AddAsync(comment);
                    break;

                default:
                    throw new InvalidCommentContextEnumException(command.CommentContext);
            }

            if (command.ParentId != Guid.Empty)
            {
                Comment parentComment = null;
                Guid replyId = Guid.NewGuid(); 

                switch (commentContext)
                {
                    case CommentContext.OrganizationEvent:
                        parentComment = await _organizationEventsCommentRepository.GetAsync(command.ParentId);
                        break;
                    case CommentContext.OrganizationPost:
                        parentComment = await _organizationPostsCommentRepository.GetAsync(command.ParentId);
                        break;
                    case CommentContext.UserEvent:
                        parentComment = await _userEventsCommentRepository.GetAsync(command.ParentId);
                        break;
                    case CommentContext.UserPost:
                        parentComment = await _userPostsCommentRepository.GetAsync(command.ParentId);
                        break;
                }

                if (parentComment is null)
                {
                    throw new ParentCommentNotFoundException(command.ParentId);
                }

                if (parentComment.ParentId != Guid.Empty)
                {
                    throw new InvalidParentCommentException(command.ParentId);
                }

                parentComment.AddReply(replyId, command.UserId, command.TextContent, now);

                switch (commentContext)
                {
                    case CommentContext.OrganizationEvent:
                        await _organizationEventsCommentRepository.UpdateAsync(parentComment);
                        break;
                    case CommentContext.OrganizationPost:
                        await _organizationPostsCommentRepository.UpdateAsync(parentComment);
                        break;
                    case CommentContext.UserEvent:
                        await _userEventsCommentRepository.UpdateAsync(parentComment);
                        break;
                    case CommentContext.UserPost:
                        await _userPostsCommentRepository.UpdateAsync(parentComment);
                        break;
                }
            }

            await _messageBroker.PublishAsync(new CommentCreated(
                comment.Id,
                comment.ContextId,
                comment.CommentContext.ToString(),
                comment.UserId,
                comment.ParentId,
                comment.TextContent,
                comment.CreatedAt,
                comment.LastUpdatedAt,
                comment.RepliesCount,
                comment.IsDeleted,
                user.FirstName + " " + user.LastName, 
                user.ProfileImageUrl
            ));
        }
    }
}
