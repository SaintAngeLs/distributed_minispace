using System;

namespace MiniSpace.Web.DTO
{
    public class FileUploadResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ProcessedUrl { get; set; }
    }
}