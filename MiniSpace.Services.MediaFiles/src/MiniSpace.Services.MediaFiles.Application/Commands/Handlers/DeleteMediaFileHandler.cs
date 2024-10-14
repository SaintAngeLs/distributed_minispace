using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Repositories;

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
            // Serialize the command to JSON and write to the console
            var commandJson = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Received DeleteMediaFile command: " + commandJson);

            // Decode the URL before using it
            var decodedUrl = Uri.UnescapeDataString(command.MediaFileUrl);
            Console.WriteLine($"DeleteMediaFileHandler: {decodedUrl}");

            // Retrieve the file source information
            var fileSourceInfo = await _fileSourceInfoRepository.GetAsync(decodedUrl);
            if (fileSourceInfo is null)
            {
                throw new MediaFileNotFoundException(decodedUrl);
            }

            // Log the fileSourceInfo details to ensure it contains the correct data
            var fileSourceInfoJson = JsonSerializer.Serialize(fileSourceInfo, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Retrieved FileSourceInfo: " + fileSourceInfoJson);

            // Check for unauthorized access
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != fileSourceInfo.UploaderId && 
                !fileSourceInfo.OrganizationId.HasValue)
            {
                throw new UnauthorizedMediaFileAccessException(fileSourceInfo.Id, identity.Id, fileSourceInfo.UploaderId);
            }

            // Delete the files from S3
            await _s3Service.DeleteFileAsync(fileSourceInfo.OriginalFileUrl);
            await _s3Service.DeleteFileAsync(fileSourceInfo.FileUrl);
            await _fileSourceInfoRepository.DeleteAsync(decodedUrl);
            
            // Publish the MediaFileDeleted event
            await _messageBroker.PublishAsync(new MediaFileDeleted(
                decodedUrl, 
                fileSourceInfo.SourceId, 
                fileSourceInfo.SourceType.ToString(), 
                fileSourceInfo.UploaderId,
                fileSourceInfo.OrganizationId,
                fileSourceInfo.EventId,  
                fileSourceInfo.PostId      
            ));
        }
    }
}
