using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler: IEventHandler<MediaFileDeleted>
    {
        private readonly IPostRepository _postRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MediaFileDeletedHandler(IPostRepository postRepository, IDateTimeProvider dateTimeProvider)
        {
            _postRepository = postRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            if(@event.Source.ToLowerInvariant() != "post")
            {
                return;
            }

            var post = await _postRepository.GetAsync(@event.SourceId);
            if (post != null)
            {
                post.RemoveMediaFile(@event.MediaFileId, _dateTimeProvider.Now);
                await _postRepository.UpdateAsync(post);
            }
        }
    }
}