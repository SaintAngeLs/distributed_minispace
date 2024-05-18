using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class CleanupUnassociatedFilesHandler: ICommandHandler<CleanupUnassociatedFiles>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IGridFSService _gridFSService;
        private readonly IMessageBroker _messageBroker;
        
        public CleanupUnassociatedFilesHandler(IFileSourceInfoRepository fileSourceInfoRepository, IGridFSService gridFSService,
            IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _gridFSService = gridFSService;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(CleanupUnassociatedFiles command, CancellationToken cancellationToken)
        {
            var unassociatedFileSourceInfos = await _fileSourceInfoRepository.GetAllUnassociatedAsync();
            foreach (var file in unassociatedFileSourceInfos)
            {
                if ((command.Now - file.CreatedAt).TotalDays < 1)
                {
                    continue;
                }

                await _gridFSService.DeleteFileAsync(file.OriginalFileId);
                await _gridFSService.DeleteFileAsync(file.FileId);
                await _fileSourceInfoRepository.DeleteAsync(file.Id);
            }
            
            await _messageBroker.PublishAsync(new UnassociatedFilesCleaned(command.Now));
        }
    }
}