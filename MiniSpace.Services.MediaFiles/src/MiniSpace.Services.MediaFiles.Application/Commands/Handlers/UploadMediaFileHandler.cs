using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class UploadMediaFileHandler: ICommandHandler<UploadMediaFile>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly IFileValidator _fileValidator;
        private readonly IGridFSService _gridFSService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public UploadMediaFileHandler(IFileSourceInfoRepository fileSourceInfoRepository, IFileValidator fileValidator, 
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
        
        public async Task HandleAsync(UploadMediaFile command, CancellationToken cancellationToken)
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
            
            using var inStream = new MemoryStream(bytes);
            using var myImage = await Image.LoadAsync(inStream, cancellationToken);
            using var outStream = new MemoryStream();
            await myImage.SaveAsync(outStream, new WebpEncoder(), cancellationToken);

            var originalObjectId = await _gridFSService.UploadFileAsync(command.FileName, inStream);
            var objectId = await _gridFSService.UploadFileAsync(command.FileName, outStream);
            var fileSourceInfo = new FileSourceInfo(command.MediaFileId, command.SourceId, sourceType, 
                command.UploaderId, State.Unassociated, _dateTimeProvider.Now, originalObjectId,objectId, command.FileName);
            await _fileSourceInfoRepository.AddAsync(fileSourceInfo);
            await _messageBroker.PublishAsync(new MediaFileUploaded(command.MediaFileId, command.FileName));
        }
    }
}