using Convey.CQRS.Commands;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class UploadMediaFileHandler: ICommandHandler<UploadMediaFile>
    {
        
        public Task HandleAsync(UploadMediaFile command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}