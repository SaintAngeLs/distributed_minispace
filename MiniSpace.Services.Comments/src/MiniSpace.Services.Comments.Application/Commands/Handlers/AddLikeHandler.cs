using System;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services.Clients;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Services;
using System.Text.Json;

namespace MiniSpace.Services.Comments.Application.Commands.Handlers
{
    public class AddLikeHandler : ICommandHandler<AddLike>
    {
        private readonly IOrganizationEventsCommentRepository _organizationEventsCommentRepository;
        private readonly IOrganizationPostsCommentRepository _organizationPostsCommentRepository;
        private readonly IUserEventsCommentRepository _userEventsCommentRepository;
        private readonly IUserPostsCommentRepository _userPostsCommentRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;
        private readonly IStudentsServiceClient _userServiceClient;

        public AddLikeHandler(
            IOrganizationEventsCommentRepository organizationEventsCommentRepository,
            IOrganizationPostsCommentRepository organizationPostsCommentRepository,
            IUserEventsCommentRepository userEventsCommentRepository,
            IUserPostsCommentRepository userPostsCommentRepository,
            IMessageBroker messageBroker,
            IAppContext appContext,
            IStudentsServiceClient userServiceClient)
        {
            _organizationEventsCommentRepository = organizationEventsCommentRepository;
            _organizationPostsCommentRepository = organizationPostsCommentRepository;
            _userEventsCommentRepository = userEventsCommentRepository;
            _userPostsCommentRepository = userPostsCommentRepository;
            _messageBroker = messageBroker;
            _appContext = appContext;
            _userServiceClient = userServiceClient;
        }

        public async Task HandleAsync(AddLike command, CancellationToken cancellationToken = default)
        {
            var commandJson = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"Received AddLike command: {commandJson}");

            var identity = _appContext.Identity;

            if (!identity.IsAuthenticated || identity.Id != command.UserId)
            {
                throw new UnauthorizedCommentAccessException(command.CommentId, identity.Id);
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

            Comment comment = await GetCommentAsync(command.CommentId, commentContext);
            if (comment == null)
            {
                throw new CommentNotFoundException(command.CommentId);
            }

            comment.Like(command.UserId);
            await UpdateCommentAsync(comment, commentContext);

            await _messageBroker.PublishAsync(new LikeAdded(
                commentId: command.CommentId,
                userId: command.UserId,
                commentContext: command.CommentContext,
                likedAt: DateTime.UtcNow,
                userName: $"{user.FirstName} {user.LastName}",  
                profileImageUrl: user.ProfileImageUrl 
            ));
        }

        private async Task<Comment> GetCommentAsync(Guid commentId, CommentContext context)
        {
            return context switch
            {
                CommentContext.OrganizationEvent => await _organizationEventsCommentRepository.GetAsync(commentId),
                CommentContext.OrganizationPost => await _organizationPostsCommentRepository.GetAsync(commentId),
                CommentContext.UserEvent => await _userEventsCommentRepository.GetAsync(commentId),
                CommentContext.UserPost => await _userPostsCommentRepository.GetAsync(commentId),
                _ => null
            };
        }

        private async Task UpdateCommentAsync(Comment comment, CommentContext context)
        {
            switch (context)
            {
                case CommentContext.OrganizationEvent:
                    await _organizationEventsCommentRepository.UpdateAsync(comment);
                    break;
                case CommentContext.OrganizationPost:
                    await _organizationPostsCommentRepository.UpdateAsync(comment);
                    break;
                case CommentContext.UserEvent:
                    await _userEventsCommentRepository.UpdateAsync(comment);
                    break;
                case CommentContext.UserPost:
                    await _userPostsCommentRepository.UpdateAsync(comment);
                    break;
                default:
                    throw new InvalidCommentContextEnumException(context.ToString());
            }
        }
    }
}
