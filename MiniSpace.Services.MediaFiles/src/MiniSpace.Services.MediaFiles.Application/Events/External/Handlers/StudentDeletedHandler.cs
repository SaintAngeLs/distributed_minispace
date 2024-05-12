using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class StudentDeletedHandler : IEventHandler<StudentDeleted>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public StudentDeletedHandler(IFileSourceInfoRepository fileSourceInfoRepository, ICommandDispatcher commandDispatcher)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(StudentDeleted @event, CancellationToken cancellationToken)
        {
            var fileSourceInfos =
                await _fileSourceInfoRepository.FindAsync(@event.StudentId, ContextType.StudentProfile);
            foreach (var fileSourceInfo in fileSourceInfos)
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