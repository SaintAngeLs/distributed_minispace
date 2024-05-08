namespace MiniSpace.Services.MediaFiles.Application.Dto
{
    public class FileDto
    {
        public Guid MediaFileId { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public string FileName { get; set; }
        public string Base64Content { get; set; }
        
        public FileDto(Guid mediaFileId, Guid sourceId, string sourceType, string fileName, string base64Content)
        {
            MediaFileId = mediaFileId == Guid.Empty ? Guid.NewGuid() : mediaFileId;
            SourceId = sourceId;
            SourceType = sourceType;
            FileName = fileName;
            Base64Content = base64Content;
        }
    }
}