using System;

namespace Astravent.Web.Wasm.DTO
{
    public class FileUploadResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ProcessedUrl { get; set; }
    }
}