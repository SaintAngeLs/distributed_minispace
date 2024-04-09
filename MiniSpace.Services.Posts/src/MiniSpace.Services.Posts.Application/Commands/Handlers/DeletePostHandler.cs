using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Commands.Handlers
{
    public class DeletePostHandler : ICommandHandler<DeletePost>
    {
        private readonly IPostRepository _postRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeletePostHandler(IPostRepository postRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _postRepository = postRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(DeletePost command, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(command.PostId);
            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.StudentId && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }
            
            await _postRepository.DeleteAsync(command.PostId);

            await _messageBroker.PublishAsync(new PostDeleted(command.PostId));
        }
    }    
}
