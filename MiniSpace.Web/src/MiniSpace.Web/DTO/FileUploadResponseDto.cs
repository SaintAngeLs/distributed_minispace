using System;

namespace MiniSpace.Web.DTO
{
    public class FileUploadResponseDto
    {
        public Guid FileId { get; set; }
        public string OriginalUrl { get; }
        public string ProcessedUrl { get; }
    }
}