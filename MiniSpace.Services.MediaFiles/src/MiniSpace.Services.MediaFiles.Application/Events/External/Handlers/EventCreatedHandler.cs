using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class EventCreatedHandler : IEventHandler<EventCreated>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;

        public EventCreatedHandler(IFileSourceInfoRepository fileSourceInfoRepository)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
        }

        public async Task HandleAsync(EventCreated @event, CancellationToken cancellationToken)
        {
            var fileSourceInfos =
                await _fileSourceInfoRepository.FindAsync(@event.EventId, ContextType.Event);
            foreach (var fileSourceInfo in fileSourceInfos)
            {
                if(@event.MediaFilesIds.Contains(fileSourceInfo.Id))
                {
                    fileSourceInfo.Associate();
                    await _fileSourceInfoRepository.UpdateAsync(fileSourceInfo);
                }
            }
        }
    }
}