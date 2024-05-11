using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class DeleteMediaFileHandler: ICommandHandler<DeleteMediaFile>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IGridFSService _gridFSService;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeleteMediaFileHandler(IFileSourceInfoRepository fileSourceInfoRepository, IGridFSService gridFSService,
            IAppContext appContext, IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _gridFSService = gridFSService;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(DeleteMediaFile command, CancellationToken cancellationToken)
        {
            var fileSourceInfo = await _fileSourceInfoRepository.GetAsync(command.MediaFileId);
            if (fileSourceInfo is null)
            {
                throw new MediaFileNotFoundException(command.MediaFileId);
            }
            
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && identity.Id != fileSourceInfo.UploaderId && !identity.IsAdmin)
            {
                throw new UnauthorizedMediaFileAccessException(fileSourceInfo.Id, identity.Id, fileSourceInfo.UploaderId);
            }
            
            await _gridFSService.DeleteFileAsync(fileSourceInfo.FileId);
            await _fileSourceInfoRepository.DeleteAsync(command.MediaFileId);
            await _messageBroker.PublishAsync(new MediaFileDeleted(command.MediaFileId, 
                fileSourceInfo.SourceId, fileSourceInfo.SourceType.ToString()));
        }
    }
}