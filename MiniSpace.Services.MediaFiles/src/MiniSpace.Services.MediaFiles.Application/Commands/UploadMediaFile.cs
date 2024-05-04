using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Http;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class UploadMediaFile : ICommand
    {
        //public Guid MediaFileId { get; }
        public IFormFile File { get;  }

        public UploadMediaFile(IFormFile file)
        {
            //MediaFileId = mediaFileId == Guid.Empty ? Guid.NewGuid() : mediaFileId;
            File = file;
        }
    }
}