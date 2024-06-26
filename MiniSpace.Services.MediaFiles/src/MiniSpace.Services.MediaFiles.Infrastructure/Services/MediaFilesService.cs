﻿using MiniSpace.Services.MediaFiles.Application;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class MediaFilesService: IMediaFilesService
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IFileValidator _fileValidator;
        private readonly IGridFSService _gridFSService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public MediaFilesService(IFileSourceInfoRepository fileSourceInfoRepository, IFileValidator fileValidator, 
            IGridFSService gridFSService, IDateTimeProvider dateTimeProvider, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _fileValidator = fileValidator;
            _gridFSService = gridFSService;
            _dateTimeProvider = dateTimeProvider;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task<FileUploadResponseDto> UploadAsync(UploadMediaFile command)
        {
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && identity.Id != command.UploaderId)
            {
                throw new UnauthorizedMediaFileUploadException(identity.Id, command.UploaderId);
            }
            
            if (!Enum.TryParse(command.SourceType, out ContextType sourceType))
            {
                throw new InvalidContextTypeException(command.SourceType);
            }
            
            byte[] bytes = Convert.FromBase64String(command.Base64Content);
            _fileValidator.ValidateFileSize(bytes.Length);
            _fileValidator.ValidateFileExtensions(bytes, command.FileContentType);
            
            using var inStream = new MemoryStream(bytes);
            using var myImage = await Image.LoadAsync(inStream);
            using var outStream = new MemoryStream();
            await myImage.SaveAsync(outStream, new WebpEncoder());
            inStream.Position = 0;
            outStream.Position = 0;

            var originalObjectId = await _gridFSService.UploadFileAsync(command.FileName, inStream);
            var objectId = await _gridFSService.UploadFileAsync(command.FileName, outStream);
            var fileSourceInfo = new FileSourceInfo(command.MediaFileId, command.SourceId, sourceType, 
                command.UploaderId, State.Unassociated, _dateTimeProvider.Now, originalObjectId, 
                command.FileContentType, objectId, command.FileName);
            await _fileSourceInfoRepository.AddAsync(fileSourceInfo);
            await _messageBroker.PublishAsync(new MediaFileUploaded(command.MediaFileId, command.FileName));

            return new FileUploadResponseDto(fileSourceInfo.Id);
        }

    }
}