using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Http;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class UploadMediaFile : ICommand
    {
        public Guid MediaFileId { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public Guid UploaderId { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public IFormFile File { get; set; } 

        public UploadMediaFile(Guid mediaFileId, Guid sourceId, string sourceType, Guid uploaderId,
            string fileName, string fileContentType, IFormFile file)
        {
            MediaFileId = mediaFileId == Guid.Empty ? Guid.NewGuid() : mediaFileId;
            SourceId = sourceId;
            SourceType = sourceType;
            UploaderId = uploaderId;
            FileName = fileName;
            FileContentType = fileContentType;
            File = file;
        }
    }
}
