﻿using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class EventUpdatedHandler : IEventHandler<EventUpdated>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public EventUpdatedHandler(IFileSourceInfoRepository fileSourceInfoRepository, ICommandDispatcher commandDispatcher)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(EventUpdated @event, CancellationToken cancellationToken)
        {
            var fileSourceInfos =
                await _fileSourceInfoRepository.FindAsync(@event.EventId, ContextType.Event);
            foreach (var fileSourceInfo in fileSourceInfos)
            {
                if(@event.MediaFilesIds.Contains(fileSourceInfo.Id))
                {
                    fileSourceInfo.Associate();
                }
                else
                {
                    fileSourceInfo.Unassociate();
                }
                await _fileSourceInfoRepository.UpdateAsync(fileSourceInfo);
            }
        }
    }
}