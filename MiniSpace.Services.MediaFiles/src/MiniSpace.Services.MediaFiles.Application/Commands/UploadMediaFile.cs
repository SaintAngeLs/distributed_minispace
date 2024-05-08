using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Http;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class UploadMediaFile : ICommand
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }

        public UploadMediaFile(string fileName, string base64Content)
        {
            FileName = fileName;
            Base64Content = base64Content;
        }
    }
}