using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class UpdatePostHandler : ICommandHandler<UpdatePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        public UpdatePostHandler(IPostRepository postRepository, IAppContext appContext,
            IMessageBroker messageBroker, IDateTimeProvider dateTimeProvider)
        {
            _postRepository = postRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task HandleAsync(UpdatePost command, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(command.PostId);
            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.OrganizerId && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }
            
            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }
            
            var mediaFiles = command.MediaFiles.ToList();
            if(mediaFiles.Count > 3)
            {
                throw new InvalidNumberOfPostMediaFilesException(post.Id, mediaFiles.Count);
            }
            
            post.Update(command.TextContent, command.MediaFiles, _dateTimeProvider.Now);
            await _postRepository.UpdateAsync(post);

            await _messageBroker.PublishAsync(new PostUpdated(command.PostId, post.MediaFiles));
        }
    }    
}
