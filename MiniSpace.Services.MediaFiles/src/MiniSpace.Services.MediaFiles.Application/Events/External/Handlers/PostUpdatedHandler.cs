using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class PostUpdatedHandler : IEventHandler<PostUpdated>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public PostUpdatedHandler(IFileSourceInfoRepository fileSourceInfoRepository, ICommandDispatcher commandDispatcher)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(PostUpdated @event, CancellationToken cancellationToken)
        {
            var fileSourceInfos =
                await _fileSourceInfoRepository.FindAsync(@event.PostId, ContextType.Post);
            foreach (var fileSourceInfo in fileSourceInfos)
            {
                if(!@event.MediaFilesIds.Contains(fileSourceInfo.Id))
                {
                    await _commandDispatcher.SendAsync(new DeleteMediaFile
                        {
                            MediaFileId = fileSourceInfo.Id
                        }, 
                        cancellationToken);
                }
            }
        }
    }
}