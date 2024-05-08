using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class UploadMediaFileHandler: ICommandHandler<UploadMediaFile>
    {
        private readonly IGridFSService _gridFSService;
        private readonly IMessageBroker _messageBroker;
        
        public UploadMediaFileHandler(IGridFSService gridFSService, IMessageBroker messageBroker)
        {
            _gridFSService = gridFSService;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(UploadMediaFile command, CancellationToken cancellationToken)
        {
            byte[] bytes = Convert.FromBase64String(command.Base64Content);
            MemoryStream stream = new MemoryStream(bytes);

            var objectId = await _gridFSService.UploadFileAsync(command.FileName, stream);
            await _messageBroker.PublishAsync(new MediaFileUploaded(Guid.Empty, command.FileName));
        }
    }
}