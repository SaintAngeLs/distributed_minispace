using MiniSpace.Services.MediaFiles.Application;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Events.External;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.UploaderId)
            {
                throw new UnauthorizedMediaFileUploadException(identity.Id, command.UploaderId);
            }

            if (!Enum.TryParse(command.SourceType, out ContextType sourceType))
            {
                throw new InvalidContextTypeException(command.SourceType);
            }

            if (sourceType == ContextType.StudentProfileImage || sourceType == ContextType.StudentBannerImage)
            {
                var existingFiles = await _fileSourceInfoRepository.FindByUploaderIdAndSourceTypeAsync(command.UploaderId, sourceType);
                foreach (var existingFile in existingFiles)
                {
                    existingFile.Unassociate();
                    await _fileSourceInfoRepository.UpdateAsync(existingFile);
                }
            }

            byte[] bytes = Convert.FromBase64String(command.Base64Content);
            _fileValidator.ValidateFileSize(bytes.Length);
            _fileValidator.ValidateFileExtensions(bytes, command.FileContentType);

            using var inStream = new MemoryStream(bytes);
            using var myImage = await Image.LoadAsync(inStream);
            using var outStream = new MemoryStream();
            await myImage.SaveAsync(outStream, new WebpEncoder { Quality = 75 });
            inStream.Position = 0;
            outStream.Position = 0;

            string originalFileName = GenerateUniqueFileName(command.SourceType, command.UploaderId, command.FileName);
            string webpFileName = GenerateUniqueFileName(command.SourceType, command.UploaderId, command.FileName, "webp");

            var originalUrl = await _s3Service.UploadFileAsync("images", originalFileName, inStream);
            var processedUrl = await _s3Service.UploadFileAsync("webps", webpFileName, outStream);

            var uploadDate = _dateTimeProvider.Now;
            var fileSourceInfo = new FileSourceInfo(command.MediaFileId, command.SourceId, sourceType, 
                command.UploaderId, State.Associated, uploadDate, originalUrl, 
                command.FileContentType, processedUrl, originalFileName);

            await _fileSourceInfoRepository.AddAsync(fileSourceInfo);
            await _messageBroker.PublishAsync(new MediaFileUploaded(command.MediaFileId, originalFileName));

            if (sourceType == ContextType.StudentProfileImage ||
                sourceType == ContextType.StudentBannerImage ||
                sourceType == ContextType.StudentGalleryImage)
            {
                var imageType = sourceType.ToString();
                var studentImageUploadedEvent = new StudentImageUploaded(command.UploaderId, processedUrl, imageType, uploadDate);
                await _messageBroker.PublishAsync(studentImageUploadedEvent);
            }

            return new FileUploadResponseDto(fileSourceInfo.Id);
        }

        private string GenerateUniqueFileName(string contextType, Guid uploaderId, string originalFileName, string extension = null)
        {
            string timestamp = _dateTimeProvider.Now.ToString("yyyyMMddHHmmssfff");
            string hashedFileName = HashFileName(originalFileName);
            string fileExtension = extension ?? Path.GetExtension(originalFileName);

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
