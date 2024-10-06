using System;

namespace Astravent.Web.Wasm.DTO
{
    public class FileDto
    {
        public Guid MediaFileId { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UploaderId { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string Base64Content { get; set; }
    }
}