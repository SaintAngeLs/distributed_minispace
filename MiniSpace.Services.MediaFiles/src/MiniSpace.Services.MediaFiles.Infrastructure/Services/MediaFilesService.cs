using MiniSpace.Services.MediaFiles.Application;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class MediaFilesService : IMediaFilesService
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IFileValidator _fileValidator;
        private readonly IS3Service _s3Service;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public MediaFilesService(IFileSourceInfoRepository fileSourceInfoRepository, IFileValidator fileValidator, 
            IS3Service s3Service, IDateTimeProvider dateTimeProvider, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _fileValidator = fileValidator;
            _s3Service = s3Service;
            _dateTimeProvider = dateTimeProvider;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task<FileUploadResponseDto> UploadAsync(UploadMediaFile command)
        {
            var commandWithoutFileData = new UploadMediaFile(
                command.MediaFileId,
                command.SourceId,
                command.SourceType,
                command.OrganizationId,
                command.UploaderId,
                command.FileName,
                command.FileContentType,
                null // Exclude the FileData
            );

            // Serialize the modified command to JSON and write to the console
            var commandJson = JsonSerializer.Serialize(commandWithoutFileData, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Received UploadMediaFile command (excluding FileData): " + commandJson);

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.UploaderId)
            {
                throw new UnauthorizedMediaFileUploadException(identity.Id, command.UploaderId);
            }

            if (!Enum.TryParse(command.SourceType, out ContextType sourceType))
            {
                throw new InvalidContextTypeException(command.SourceType);
            }

            // Handle previous files if necessary
            if (sourceType == ContextType.StudentProfileImage || 
                sourceType == ContextType.StudentBannerImage ||
                sourceType == ContextType.OrganizationProfileImage ||
                sourceType == ContextType.OrganizationBannerImage ||
                sourceType == ContextType.EventBanner)
            {
                var existingFiles = await _fileSourceInfoRepository.FindByUploaderIdAndSourceTypeAsync(command.UploaderId, sourceType);
                foreach (var existingFile in existingFiles)
                {
                    existingFile.Unassociate();
                    await _fileSourceInfoRepository.UpdateAsync(existingFile);
                }
            }

            _fileValidator.ValidateFileSize(command.FileData.Length);
            
            // Extract the first 8 bytes for validation
            byte[] buffer = new byte[8];
            Array.Copy(command.FileData, 0, buffer, 0, Math.Min(buffer.Length, command.FileData.Length));
            _fileValidator.ValidateFileExtensions(buffer, command.FileContentType);

            // Load the image from the byte array
            using var inStream = new MemoryStream(command.FileData);
            using var myImage = await Image.LoadAsync(inStream);

            // Process the image (e.g., resizing)
            using var outStream = new MemoryStream();
            myImage.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1024, 1024) // Adjust size for optimization
            }));
            await myImage.SaveAsync(outStream, new WebpEncoder { Quality = 50 });

            // Generate unique file names
            string originalFileName = GenerateUniqueFileName(command.SourceType, command.UploaderId, command.FileName);
            string webpFileName = GenerateUniqueFileName(command.SourceType, command.UploaderId, command.FileName, "webp");

            // Upload original and processed files to S3
            var originalUrlTask = _s3Service.UploadFileAsync("images", originalFileName, inStream);
            var processedUrlTask = _s3Service.UploadFileAsync("webps", webpFileName, outStream);

            await Task.WhenAll(originalUrlTask, processedUrlTask);

            var originalUrl = await originalUrlTask;
            var processedUrl = await processedUrlTask;

            // Store file info in the repository
            var uploadDate = _dateTimeProvider.Now;
            var fileSourceInfo = new FileSourceInfo(command.MediaFileId, command.SourceId, sourceType, 
                command.UploaderId, State.Associated, uploadDate, originalUrl, 
                command.FileContentType, processedUrl, originalFileName, command.OrganizationId);

            await _fileSourceInfoRepository.AddAsync(fileSourceInfo);
            await _messageBroker.PublishAsync(new MediaFileUploaded(command.MediaFileId, originalFileName));

            // Handle specific events based on the source type
            if (command.OrganizationId.HasValue)
            {
                var imageType = sourceType.ToString();
                var organizationImageUploadedEvent = new OrganizationImageUploaded(command.OrganizationId.Value, processedUrl, imageType, uploadDate);
                await _messageBroker.PublishAsync(organizationImageUploadedEvent);
            }
            else if (sourceType == ContextType.StudentProfileImage ||
                     sourceType == ContextType.StudentBannerImage ||
                     sourceType == ContextType.StudentGalleryImage)
            {
                var imageType = sourceType.ToString();
                var studentImageUploadedEvent = new StudentImageUploaded(command.UploaderId, processedUrl, imageType, uploadDate);
                await _messageBroker.PublishAsync(studentImageUploadedEvent);
            }
            else if (sourceType == ContextType.EventBanner || 
                     sourceType == ContextType.EventGalleryImage)
            {
                var imageType = sourceType.ToString();
                var eventImageUploadedEvent = new EventImageUploaded(command.SourceId, processedUrl, imageType, uploadDate);
                await _messageBroker.PublishAsync(eventImageUploadedEvent);
            }

            return new FileUploadResponseDto(fileSourceInfo.Id);
        }

        private string GenerateUniqueFileName(string contextType, Guid uploaderId, string originalFileName, string extension = null)
        {
            string timestamp = _dateTimeProvider.Now.ToString("yyyyMMddHHmmssfff");
            string hashedFileName = HashFileName(originalFileName);
            string fileExtension = extension ?? Path.GetExtension(originalFileName);

            if (!fileExtension.StartsWith("."))
            {
                fileExtension = "." + fileExtension;
            }

            return $"{contextType}_{uploaderId}_{timestamp}_{hashedFileName}{fileExtension}";
        }

        private string HashFileName(string fileName)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fileName));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
