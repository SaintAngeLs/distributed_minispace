namespace MiniSpace.Services.MediaFiles.Application.Dto
{
    public class FileUploadResponseDto
    {
        public Guid Id { get; }
        public string OriginalUrl { get; }
        public string ProcessedUrl { get; }

        public FileUploadResponseDto(Guid id, string originalUrl, string processedUrl)
        {
            Id = id;
            OriginalUrl = originalUrl;
            ProcessedUrl = processedUrl;
        }
    }
}
