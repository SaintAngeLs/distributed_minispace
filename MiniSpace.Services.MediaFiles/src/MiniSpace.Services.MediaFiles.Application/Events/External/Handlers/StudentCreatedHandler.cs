using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class StudentCreatedHandler : IEventHandler<StudentCreated>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public StudentCreatedHandler(IFileSourceInfoRepository fileSourceInfoRepository, ICommandDispatcher commandDispatcher)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(StudentCreated @event, CancellationToken cancellationToken)
        {
            var fileSourceInfos =
                await _fileSourceInfoRepository.FindAsync(@event.StudentId, ContextType.StudentProfile);
            foreach (var fileSourceInfo in fileSourceInfos)
            {
                if (fileSourceInfo.Id == @event.MediaFileId)
                {
                    fileSourceInfo.Associate();
                    await _fileSourceInfoRepository.UpdateAsync(fileSourceInfo);
                }
            }
        }
    }
}