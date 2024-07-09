using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class CleanupUnassociatedFilesHandler : ICommandHandler<CleanupUnassociatedFiles>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IS3Service _s3Service;
        private readonly IMessageBroker _messageBroker;

        public CleanupUnassociatedFilesHandler(IFileSourceInfoRepository fileSourceInfoRepository, IS3Service s3Service,
            IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _s3Service = s3Service;
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

                await _s3Service.DeleteFileAsync(file.OriginalFileUrl);
                await _s3Service.DeleteFileAsync(file.FileUrl);
                await _fileSourceInfoRepository.DeleteAsync(file.Id);
            }

            await _messageBroker.PublishAsync(new UnassociatedFilesCleaned(command.Now));
        }
    }
}
