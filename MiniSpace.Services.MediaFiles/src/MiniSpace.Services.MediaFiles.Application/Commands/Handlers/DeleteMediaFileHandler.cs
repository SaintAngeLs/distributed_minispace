using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class DeleteMediaFileHandler : ICommandHandler<DeleteMediaFile>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IS3Service _s3Service;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeleteMediaFileHandler(IFileSourceInfoRepository fileSourceInfoRepository, IS3Service s3Service,
            IAppContext appContext, IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _s3Service = s3Service;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteMediaFile command, CancellationToken cancellationToken)
        {
            var fileSourceInfo = await _fileSourceInfoRepository.GetAsync(command.MediaFileUrl);
            if (fileSourceInfo is null)
            {
                throw new MediaFileNotFoundException(command.MediaFileUrl);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != fileSourceInfo.UploaderId)
            {
                throw new UnauthorizedMediaFileAccessException(fileSourceInfo.Id, identity.Id, fileSourceInfo.UploaderId);
            }

            await _s3Service.DeleteFileAsync(fileSourceInfo.OriginalFileUrl);
            await _s3Service.DeleteFileAsync(fileSourceInfo.FileUrl);
            await _fileSourceInfoRepository.DeleteAsync(command.MediaFileUrl);
            await _messageBroker.PublishAsync(new MediaFileDeleted(command.MediaFileUrl, 
                fileSourceInfo.SourceId, fileSourceInfo.SourceType.ToString()));
        }
    }
}
